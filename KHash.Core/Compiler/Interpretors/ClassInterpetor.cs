using Force.DeepCloner;
using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Compiler.Scope;
using KHash.Core.Exceptions;
using KHash.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Interpretors
{
    public partial class Interpretor
    {
        private void ClassDecleration( ClassDeclr ast )
        {
            ClassScope classScope = new ClassScope()
            {
                Type = ast.Name.TokenValue,
                ClassAST = ast
            };
            container.Current().SetMemoryValue( classScope.Type, classScope );
        }

        private dynamic ClassInvoke( ClassInvoke ast )
        {
            ClassScope classScope = GetClassFromMemory( ast.Name );
            if( classScope != null && classScope.ClassAST != null )
            {
                if( classScope.ClassAST.MagicMethods.Constructor != null )
                {
                    ConstructorInvoke( ast, classScope );
                }else
                {
                    SettingClassScope( classScope );
                }

                return container.EndScope().Parent;
            }

            return null;
        }

        private ClassScope GetClassFromMemory( Token name )
        {
            var className = name.TokenValue;
            var value = container.Current().GetMemoryValue( className );
            if( value == null )
            {
                value = container.Global().GetMemoryValue( className );
            }

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
            container.Current().SetMemoryValue( symbol, memoryValue, true );
        }

        public void MethodDecleration( MethodDeclr ast )
        {
            MemoryValue memoryValue = new MemoryValue()
            {
                Type = ast.GetType(),
                Value = ast,
                AccessModifier = ast.AccessModifier.TokenType
            };
            container.Current().SetMemoryValue( ast.Name.TokenValue, memoryValue, true);
        }

        public void SettingClassScope( ClassScope classScope )
        {
            var current = container.StartScope();
            current.Parent = classScope.DeepClone();

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

        private dynamic ClassReference( ClassReference ast )
        {
            ClassScope classScope = GetClassFromMemory( ast.Token );
            if( classScope != null && classScope.ClassAST != null )
            {
                object currentValueToReturn = null;
                foreach( var reference in ast.Deferences )
                {
                    if( currentValueToReturn != null && currentValueToReturn is ClassScope )
                    {
                        classScope = (ClassScope)currentValueToReturn;
                    }

                    //check if ref is public
                    string keyName = reference.Token.TokenValue;
                    bool isProperty = true;
                    if( reference is FunctionInvoke )
                    {
                        var o = (FunctionInvoke)reference;
                        keyName = o.Name.TokenValue;
                        isProperty = false;
                    }

                    if( classScope.Memory.ContainsKey( keyName ) && 
                        classScope.Memory[ keyName ].AccessModifier != TokenType.Public )
                    {
                        string typeMessage = isProperty ? "property" : "method";
                        throw new InterpretorException( String.Format("Cannot access {0}, {1} must be defined as public, {2} set in class {3}", keyName, typeMessage, classScope.Memory[keyName].AccessModifier.ToString().ToLower(), classScope.ClassAST.Name.TokenValue ) );
                    }

                    var current = container.StartScope();
                    current.Parent = classScope.DeepClone();

                    currentValueToReturn = Execute( reference );

                    container.EndScope();
                }
                return currentValueToReturn;
            }
            return null;
        }
    }

    
}
