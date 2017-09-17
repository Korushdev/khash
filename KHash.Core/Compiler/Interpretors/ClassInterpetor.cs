using Force.DeepCloner;
using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Compiler.Scope;
using KHash.Core.Exceptions;
using KHash.Core.Helpers;
using KHash.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Interpretors
{
    public partial class Interpretor
    {
        private void ClassDecleration( ClassDeclr ast )
        {
            ClassScope classScope = new ClassScope( this.container, ast.Token.TokenValue+" Class decleration")
            {
                Type = ast.Name.TokenValue,
                ClassAST = ast
            };
            

            if( ast.IsStatic )
            {
                classScope.IsInstantiated = true;
                SettingClassScope( classScope );
                var s = container.Current.EndScope();

                container.Current.SetMemoryValue( classScope.Type, s.Parent);
            }
            else
            {
                container.Current.SetMemoryValue( classScope.Type, classScope );
            }
        }

        private dynamic ClassInvoke( ClassInvoke ast )
        {
            ClassScope classScope = GetClassFromMemory( ast.Name );
            if( classScope != null && classScope.ClassAST != null )
            {
                if( classScope.ClassAST.IsStatic )
                {
                    throw new ClassException( String.Format( "Class {0} is static and cannot be instatiated", classScope.ClassAST.Name.TokenValue ) );
                }
                classScope.IsInstantiated = true;
                if( classScope.ClassAST.MagicMethods.Constructor != null )
                {
                    ConstructorInvoke( ast, classScope );
                }else
                {
                    SettingClassScope( classScope );
                }

                var endedClassScope = container.Current.EndScope();
                return endedClassScope.ParentClassScope;
            }

            return null;
        }

        private ClassScope GetFromLibraries( Token name )
        {
            var tuple = this.Libraries.GetClass( name );

            if( tuple != null )
            {
                ClassLibraryScope classScope = new ClassLibraryScope( this.container, "Calling class lib"+name )
                {
                    IsInstantiated = true,
                    Type = tuple.Item2.GetName(),
                    ClassDef = tuple.Item2,
                    Library = tuple.Item1
                };
                return classScope;
            }

            return null;
        }

        private ClassScope GetClassFromMemory( Token name )
        {
            ClassScope libraryClass = GetFromLibraries( name );
            if( libraryClass != null )
            {
                return libraryClass;
            }
            var className = name.TokenValue;
            var value = container.Current.GetMemoryValue( className );
            

            if( value is ClassScope )
            {
                return (ClassScope)value;
            }
            return null;
        }


        private void PropertyDecleration( PropertyDeclr ast )
        {
            var variableValue = ast.VariableValue;

            var propertyValue = Execute( variableValue, ast.DeclarationType );

            var symbol = ast.VariableName.Token.TokenValue;

            var castedValue = TypeHelper.CastByString( ast.DeclarationType.Token.TokenValue, propertyValue );
            MemoryValue memoryValue = new MemoryValue()
            {
                Type = castedValue.GetType(),
                Value = castedValue,
                AccessModifier = ast.AccessModifier.TokenType
            };
            container.Current.SetMemoryValue( symbol, memoryValue, true );
        }

        public void MethodDecleration( MethodDeclr ast )
        {
            MemoryValue memoryValue = new MemoryValue()
            {
                Type = ast.GetType(),
                Value = ast,
                AccessModifier = ast.AccessModifier.TokenType
            };
            container.Current.SetMemoryValue( ast.Name.TokenValue, memoryValue, true);
        }

        public void SettingClassScope( ClassScope classScope )
        {
            var current = container.Current.StartScope();
            current.ParentClassScope = classScope.DeepClone();

            classScope.ClassAST.Body.ScopedStatements.ForEach( statement =>
            {
                if( statement.AstType == AstTypes.PropertyDeclr || statement.AstType == AstTypes.MethodDeclr )
                {
                    Execute( statement );
                }
            } );
        }

        private void ConstructorInvoke( ClassInvoke classInvoke, ClassScope classScope )
        {
            MagicMethodDeclr ast = classScope.ClassAST.MagicMethods.Constructor;
            Action settingScope = () =>
            {
                SettingClassScope( classScope );
            };

            SetArgumentsToMemory( classInvoke.Name, ast.Arguments, classInvoke.Arguments, settingScope );
            Execute( ast.Body );
        }

        private dynamic ClassReference( ClassReference ast, bool returnScope = false )
        {
            ClassScope classScope = GetClassFromMemory( ast.Token );
            if( classScope == null )
            {
                return null;
            }
            else if( classScope is ClassLibraryScope )
            {
                return ClassLibraryReference( (ClassLibraryScope)classScope, ast, returnScope );
            }
            else if( classScope.ClassAST != null )
            {
                if( classScope.IsInstantiated == false )
                {
                    throw new ClassException( String.Format( "Class {0} is not instatiated, use the new keyword to create an object instance", classScope.ClassAST.Name.TokenValue ) );
                }
                object currentValueToReturn = null;
                foreach( var reference in ast.Deferences )
                {
                    if( currentValueToReturn != null && currentValueToReturn is ClassScope )
                    {
                        classScope = (ClassScope)currentValueToReturn;
                    }

                    //check if ref is public
                    ValidateReference( reference, classScope );
                    
                    Scope.Scope newScope = new Scope.Scope( this.container, "Ref: "+ast.Token.TokenValue+" class");
                    newScope.ParentClassScope = classScope;
                    container.Current.StartScope( newScope );

                    object resultingScope = null;
                    try
                    {
                        var result = Execute( reference );

                        currentValueToReturn = result;
                        resultingScope = container.Current.ParentClassScope;
                    }
                    catch( ParentScopeReturnException resultingScopeException )
                    {
                        resultingScope = resultingScopeException.Value;
                    }


                    container.Current.EndScope();

                    if( returnScope )
                    {
                        return resultingScope;
                    }
                    if( resultingScope is ClassScope )
                    {
                        classScope = (ClassScope)resultingScope;
                        string className = ast.Token.TokenValue;
                        if( container.Current.GetMemoryValue( className ) != null )
                        {
                            container.Current.SetMemoryValue( className,classScope );
                        }
                    }
                }
                
                return currentValueToReturn;
            }
            return null;
        }

        private dynamic ClassLibraryReference( ClassLibraryScope classScope, ClassReference ast, bool returnScope = false )
        {
            foreach( var reference in ast.Deferences )
            {
                switch( reference.AstType )
                {
                    case AstTypes.FunctionInvoke:
                        return LibraryDispatcher.Invoke( classScope.Library, classScope.ClassDef, (FunctionInvoke)reference );
                }
            }
            return null;
        }

        private void ValidateReference( AST reference, ClassScope classScope )
        {
            string keyName = reference.Token.TokenValue;
            bool isProperty = true;
            if( reference is FunctionInvoke )
            {
                var o = (FunctionInvoke)reference;
                keyName = o.Name.TokenValue;
                isProperty = false;
            }

            if( classScope.Memory.ContainsKey( keyName ) &&
                classScope.Memory[keyName].AccessModifier != TokenType.Public )
            {
                string typeMessage = isProperty ? "property" : "method";
                throw new InterpretorException( String.Format( "Cannot access {0}, {1} must be defined as public, {2} set in class {3}", keyName, typeMessage, classScope.Memory[keyName].AccessModifier.ToString().ToLower(), classScope.ClassAST.Name.TokenValue ) );
            }
        }
    }

    
}
