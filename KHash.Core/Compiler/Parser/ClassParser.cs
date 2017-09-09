using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Parser
{
    public partial class KHashParser
    {

        private AST.AST ClassInnerStatement()
        {
            var ast = MethodDecleration()
                .Or( PropertyDeclarationAndAssignment )
                .Or( MagicMethodDeclaration );

            if( ast != null )
            {
                return ast;
            }

            throw new ParserInvalidSyntaxException( String.Format( "Unknown expression type {0} - {1} found in class", tokenStream.Current.TokenType, tokenStream.Current.TokenValue ) );
        }

        private AST.AST ClassDecleration()
        {
            return tokenStream.Capture( ParseClassDecleration );
        }

        private AST.AST ParseClassDecleration()
        {
            tokenStream.Take( TokenType.Class );
            Token className = tokenStream.Take( TokenType.Word );
            var body = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket, ClassInnerStatement );
            ClassDeclr classDecleration = new ClassDeclr( className, body );

            SetMagicMethodsToClassDecleration( ref classDecleration, body );

            return classDecleration;
        }


        private AST.AST PropertyDeclarationAndAssignment()
        {
            if( IsValidAccessModifier() )
            {
                var accessModifier = tokenStream.Take( tokenStream.Current.TokenType );
                if( ( IsValidMethodReturnType() ) && IsValidVariableName( tokenStream.Peek( 1 ) ) )
                {
                    var type = tokenStream.Take( tokenStream.Current.TokenType );

                    var name = tokenStream.Take( TokenType.Word );

                    AST.AST expr = null;
                    if( tokenStream.Current.TokenType == TokenType.Equals )
                    {
                        tokenStream.Take( TokenType.Equals );

                        expr = InnerStatement();
                    }

                    return new PropertyDeclr( accessModifier, type, name, expr );
                }
            }
            
            return null;
        }

        private AST.AST MethodDecleration()
        {
            if( IsValidAccessModifier() == false || tokenStream.Peek( 3 ).TokenType != TokenType.OpenParenth )
            {
                return null;
            }

            return ParseMethodDecleration();
        }

        private AST.AST ParseMethodDecleration()
        {
            var accessModifier = tokenStream.Take( tokenStream.Current.TokenType );
            if( IsValidMethodReturnType() )
            {
                var type = tokenStream.Take( tokenStream.Current.TokenType );

                var functionName = tokenStream.Take( TokenType.Word );

                var arguments = GetArgumentList();

                var body = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket );

                return new MethodDeclr( accessModifier, functionName, type, arguments, body );
            }
            return null;
        }

        private AST.AST MagicMethodDeclaration()
        {
            if( IsValidMagicMethod() )
            {
                Token current = tokenStream.Current;
                tokenStream.Take( current.TokenType );

                var arguments = GetArgumentList();

                var body = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket );

                return new MagicMethodDeclr( current, arguments, body );
            }
            return null;
        }

        private void SetMagicMethodsToClassDecleration( ref ClassDeclr classDecleration, ScopeDeclr body )
        {
            if( classDecleration != null )
            {
                foreach( AST.AST ast in body.ScopedStatements )
                {
                    switch( ast.Token.TokenType )
                    {
                        case TokenType.Construct:
                            if( classDecleration.MagicMethods.Constructor != null )
                            {
                                throw new MethodException( "Init is already defined" );
                            }
                            classDecleration.MagicMethods.Constructor = (MagicMethodDeclr)ast;
                            break;
                        case TokenType.Destruct:
                            if( classDecleration.MagicMethods.Destructor != null )
                            {
                                throw new MethodException( "Destroy is already defined" );
                            }
                            classDecleration.MagicMethods.Destructor = (MagicMethodDeclr)ast;
                            break;
                    }
                }
            }
        }

        private AST.AST ClassInvokeStatement()
        {
            return tokenStream.Capture( ClassInvoke );
        }

        private AST.AST ClassInvoke()
        {
            tokenStream.Take( TokenType.New );
            var nameOfFunction = tokenStream.Take( TokenType.Word );

            var arguments = GetArgumentList( false );

            return new ClassInvoke( nameOfFunction, arguments );
        }

        private AST.AST ClassReferenceStatement()
        {
            Func<AST.AST> reference = () =>
            {
                var references = new List<AST.AST>();

                var classInstance = ClassInvokeStatement().Or( () => new Expr( tokenStream.Take( TokenType.Word ) ) );

                while( true )
                {
                    if( tokenStream.Current.TokenType == TokenType.Dot )
                    {
                        tokenStream.Take( TokenType.Dot );
                    }
                    else
                    {
                        if( references.Count == 0 )
                        {
                            return null;
                        }

                        if( references.Count > 0 )
                        {
                            return new ClassReference( classInstance, references );
                        }
                    }

                    var deref = FunctionCallStatement().Or( () => tokenStream.Current.TokenType == TokenType.Word ? new Expr( tokenStream.Take( TokenType.Word ) ) : null );

                    references.Add( deref );
                }
            };

            return tokenStream.Capture( reference );
        }

        private bool IsValidAccessModifier()
        {
            switch( tokenStream.Current.TokenType )
            {
                case TokenType.Private:
                case TokenType.Protected:
                case TokenType.Public:
                    return true;
            }
            return false;
        }

        private bool IsValidMagicMethod()
        {
            switch( tokenStream.Current.TokenType )
            {
                case TokenType.Construct:
                case TokenType.Destruct:
                    return true;
            }
            return false;
        }
    }
    
}
