using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Exceptions;
using KHash.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Parser
{
    public partial class KHashParser
    {
        private AST.AST Expression()
        {
            if( IsValidOperand() || tokenStream.Current.TokenType == TokenType.New )
            {
                return ParseExpression();
            }

            switch( tokenStream.Current.TokenType )
            {
                case TokenType.Increment:
                case TokenType.Decrement:
                    Func<Func<AST.AST>, Func<AST.AST>, AST.AST> incDecOP = ( leftFunc, rightFunc ) =>
                    {
                        var opType = Operator();

                        AST.AST right = null;
                        right = rightFunc();

                        if( right == null )
                        {
                            return null;
                        }

                        return new Expr( null, opType, right );
                    };

                    Func<AST.AST> leftOp = () => incDecOP( ExpressionTerminal, Expression );

                    return tokenStream.Capture( leftOp )
                                      .Or( () => tokenStream.Capture( ExpressionTerminal ) );
                case TokenType.OpenParenth:

                    Func<AST.AST> basicOp = () =>
                    {
                        tokenStream.Take( TokenType.OpenParenth );

                        var expr = Expression();

                        tokenStream.Take( TokenType.CloseParenth );

                        return expr;
                    };

                    Func<AST.AST> doubleOp = () =>
                    {
                        var op1 = basicOp();

                        var op = Operator();

                        var expr = Expression();

                        return new Expr( op1, op, expr );
                    };

                    return tokenStream.Capture( doubleOp )
                                      .Or( () => tokenStream.Capture( basicOp ) );

                default:
                    return null;
            }
        }

        private AST.AST ParseExpression()
        {
            Func<Func<AST.AST>, Func<AST.AST>, AST.AST> op = ( leftFunc, rightFunc ) =>
            {
                var left = leftFunc();

                if( left == null )
                {
                    return null;
                }

                var opType = Operator();
                AST.AST right = null;
                if( TokenHelper.HasNoRightExpression( opType.TokenType ) == false )
                {
                    right = rightFunc();

                    if( right == null )
                    {
                        return null;
                    }
                }

                return new Expr( left, opType, right );
            };

            Func<AST.AST> leftOp = () => op( ExpressionTerminal, Expression );

            return tokenStream.Capture( leftOp )
                              .Or( () => tokenStream.Capture( ExpressionTerminal ) );
        }

        private AST.AST ExpressionTerminal()
        {
            return ClassReferenceStatement()
                .Or(ClassInvokeStatement)
                .Or( FunctionCallStatement )
                .Or( SingleToken );
        }

        private AST.AST SingleToken()
        {
            if( IsValidOperand() )
            {
                var token = new Expr( tokenStream.Take( tokenStream.Current.TokenType ) );

                return token;
            }

            return null;
        }

        private Token Operator()
        {
            if( TokenHelper.IsOperator( tokenStream.Current ) )
            {
                return tokenStream.Take( tokenStream.Current.TokenType );
            }

            throw new ParserInvalidSyntaxException( String.Format( "Invalid token found. Expected operator but found {0} - {1}", tokenStream.Current.TokenType, tokenStream.Current.TokenValue ) );
        }

        private bool IsValidOperand()
        {
            switch( tokenStream.Current.TokenType )
            {
                case TokenType.Int:
                case TokenType.QuotedString:
                case TokenType.Word:
                case TokenType.True:
                case TokenType.Float:
                case TokenType.Nil:
                case TokenType.False:
                    return true;
            }
            return false;
        }
    }
}
