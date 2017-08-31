using KHash.Compiler.Lexer;
using KHash.Compiler.Parser.AST;
using KHash.Exceptions;
using KHash.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Compiler.Parser
{
    public static class Maybe
    {
        public static TInput Or<TInput>( this TInput input, Func<TInput> evaluator )
            where TInput : class
        {
            if( input != null )
            {
                return input;
            }

            return evaluator();
        }
    }

    public class KHashParser
    {
        ParserTokenizer tokenStream;

        public KHashParser( List<Token> tokens )
        {
            this.tokenStream = new ParserTokenizer( tokens );            
        }

        public AST.AST Parse()
        {
            var statements = new List<AST.AST>( 1024 );

            while( tokenStream.Current.TokenType != TokenType.EOF )
            {
                statements.Add( ScopeStart().Or( Statement ) );
            }

            return new ScopeDeclr( statements );
        }

        private AST.AST Statement()
        {
            //Check for classes or methods
            var ast = tokenStream.Capture( MethodDecleration );
            
            if( ast != null )
            {
                return ast;
            }

            ast = InnerStatement();

            if( tokenStream.Current.TokenType == TokenType.SemiColon )
            {
                tokenStream.Take( TokenType.SemiColon );
            }

            return ast;
        }

        private AST.AST ScopeStart()
        {
            if( tokenStream.Current.TokenType == TokenType.LBracket )
            {
                var statements = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket );

                return statements;
            }

            return null;
        }

        private AST.AST InnerStatement()
        {
            var ast = VariableDeclarationAndAssignment()
                .Or( SendStatement )
                .Or( ConditionalIf )
                .Or( Switch )
                .Or( While )
                .Or( Return )
                .Or( Expression );

            if( ast != null )
            {
                return ast;
            }

            throw new ParserInvalidSyntaxException( String.Format( "Unknown expression type {0} - {1}", tokenStream.Current.TokenType, tokenStream.Current.TokenValue ) );
        }

        private AST.AST MethodDecleration()
        {
            if( IsValidMethodReturnType() == false )
            {
                throw new Exception();
            }

            return ParseMethodDecleration();
        }

        private AST.AST ParseMethodDecleration()
        {
            var type = tokenStream.Take( tokenStream.Current.TokenType );
            tokenStream.Take( TokenType.Function );
            var functionName = tokenStream.Take( TokenType.Word );

            var arguments = GetArgumentList();

            var body = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket );

            return new MethodDeclr( functionName, type, arguments, body );
        }

        private AST.AST MethodInvoke()
        {
            return null;
        }

        private List<AST.AST> GetArgumentList()
        {
            tokenStream.Take( TokenType.OpenParenth );

            List<AST.AST> args = new List<AST.AST>();
            while( tokenStream.Current.TokenType != TokenType.CloseParenth )
            {
                var argument = VariableDeclaration();

                if( tokenStream.Current.TokenType == TokenType.Comma )
                {
                    tokenStream.Take( tokenStream.Current.TokenType );
                }
                args.Add( argument );
            }

            tokenStream.Take( TokenType.CloseParenth );
            return args;
        }

        private AST.AST While()
        {
            if(tokenStream.Current.TokenType == TokenType.While)
            {
                return tokenStream.Capture( ParseWhile );
            }

            return null;
        }

        private AST.AST ParseWhile()
        {
            var expressionAndStatements = GetExpressionAndStatements( TokenType.While );

            return new While( new Token( TokenType.While ), expressionAndStatements.Item1, expressionAndStatements.Item2 );
        }

        private AST.AST ConditionalIf()
        {
            if( tokenStream.Current.TokenType == TokenType.If )
            {
                return tokenStream.Capture( ParseConditionalIf );
            }
            return null;
        }

        private AST.AST ParseConditionalIf()
        {
            var expressionAndStatements = GetExpressionAndStatements( TokenType.If );

            return new Conditional( new Token( TokenType.If ), expressionAndStatements.Item1, expressionAndStatements.Item2 );
            
        }

        private Tuple<AST.AST, ScopeDeclr> GetExpressionAndStatements( TokenType typeOfLogical )
        {
            tokenStream.Take( typeOfLogical );
            tokenStream.Take( TokenType.OpenParenth );

            var expression = Expression();

            tokenStream.Take( TokenType.CloseParenth );

            var body = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket );

            return new Tuple<AST.AST, ScopeDeclr>( expression, body );
        }

        private AST.AST Switch()
        {
            if( tokenStream.Current.TokenType == TokenType.Switch )
            {
                return tokenStream.Capture( ParseSwitch );
            }
            return null;
        }

        private AST.AST ParseSwitch()
        {
            tokenStream.Take( TokenType.Switch );
            tokenStream.Take( TokenType.OpenParenth );

            var expression = Expression();

            tokenStream.Take( TokenType.CloseParenth );

            tokenStream.Take( TokenType.LBracket );
            var cases = new List<Case>();
            while( tokenStream.Current.TokenType != TokenType.RBracket )
            {
                Case caseAST = ParseCase();
                if( caseAST != null )
                {
                    cases.Add( caseAST );
                }
            }

            tokenStream.Take( TokenType.RBracket );

            return new Switch( expression, cases );
        }

        private Case ParseCase()
        {
            bool isCaseOf = false;
            if( tokenStream.Current.TokenType == TokenType.CaseOf )
            {
                isCaseOf = true;
                tokenStream.Take( TokenType.CaseOf );
            }
            else
            {
                tokenStream.Take( TokenType.Case );
            }
            var caseExpression = Expression();

            if( caseExpression == null )
            {
                throw new ParserInvalidSyntaxException( String.Format( "Unknown expression type {0} - {1}", tokenStream.Current.TokenType, tokenStream.Current.TokenValue ) );
            }

            var body = GetStatementsInScope( TokenType.Colon, TokenType.Break );
            Case caseAST = new Case( caseExpression, body, isCaseOf );

            tokenStream.Take( TokenType.SemiColon );
            return caseAST;
        }

        private AST.AST Return()
        {
            if( tokenStream.Current.TokenType == TokenType.Return && tokenStream.Alt( ParseReturn ) )
            {
                return tokenStream.Get( ParseReturn );
            }

            return null;
        }

        private Return ParseReturn()
        {
            tokenStream.Take( TokenType.Return );

            if( tokenStream.Current.TokenType == TokenType.SemiColon )
            {
                return new Return();
            }

            return new Return( InnerStatement() );
        }

        private AST.AST VariableDeclarationAndAssignment()
        {
            var isVar = tokenStream.Current.TokenType == TokenType.Var;

            if( ( isVar || IsValidMethodReturnType() ) && IsValidVariableName( tokenStream.Peek( 1 ) ) )
            {
                var type = tokenStream.Take( tokenStream.Current.TokenType );

                var name = tokenStream.Take( TokenType.Word );

                tokenStream.Take( TokenType.Equals );
                
                var expr = InnerStatement();
                
                return new VarDeclr( type, name, expr );
            }

            return null;
        }

        private AST.AST VariableDeclaration()
        {
            if( IsValidMethodReturnType() && IsValidVariableName( tokenStream.Peek( 1 ) ) )
            {
                var type = tokenStream.Take( tokenStream.Current.TokenType );

                var name = tokenStream.Take( TokenType.Word );

                return new VarDeclr( type, name );
            }

            return null;
        }

        private AST.AST VariableAssignment()
        {
            if( IsValidMethodReturnType() && IsValidVariableName( tokenStream.Peek(1) ) )
            {
                var name = tokenStream.Take( TokenType.Word );

                var equals = tokenStream.Take( TokenType.Equals );

                return new Expr( new Expr( name ), equals, InnerStatement() );
            }
            return null;
        }

        

        private AST.AST SendStatement()
        {
            Func<AST.AST> op = () =>
            {
                tokenStream.Take( TokenType.Send );

                var expr = InnerStatement();

                if( expr != null )
                {
                    return new SendAST( expr );
                }

                return null;
            };

            if( tokenStream.Alt( op ) )
            {
                return tokenStream.Get( op );
            }

            return null;
        }

        private ScopeDeclr GetStatementsInScope( TokenType startToken, TokenType endToken, bool expectSemicolon = true )
        {
            return GetStatementsInScope( startToken, endToken, InnerStatement, expectSemicolon );
        }

        private ScopeDeclr GetStatementsInScope( TokenType startToken, TokenType endToken, Func<AST.AST> getter, bool expectSemicolon = true )
        {
            tokenStream.Take( startToken );
            var lines = new List<AST.AST>();
            while( tokenStream.Current.TokenType != endToken )
            {
                var statement = getter();

                lines.Add( statement );

                if( expectSemicolon && StatementExpectsSemiColon( statement ) )
                {
                    tokenStream.Take( TokenType.SemiColon );
                }
            }

            tokenStream.Take( endToken );

            return new ScopeDeclr( lines );
        }

        private bool StatementExpectsSemiColon( AST.AST statement )
        {
            return true;
        }

        private AST.AST Expression()
        {
            if( IsValidOperand() )
            {
                return ParseExpression();
            }

            switch( tokenStream.Current.TokenType )
            {
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

                var right = rightFunc();

                if( right == null )
                {
                    return null;
                }

                return new Expr( left, opType, right );
            };

            Func<AST.AST> leftOp = () => op( ExpressionTerminal, Expression );

            return tokenStream.Capture( leftOp )
                              .Or( () => tokenStream.Capture( ExpressionTerminal ) );
        }

        private AST.AST ExpressionTerminal()
        {
            return FunctionCallStatement().Or(SingleToken);
        }

        private AST.AST FunctionCallStatement()
        {
            return tokenStream.Capture( FunctionCall );
        }

        private AST.AST FunctionCall()
        {
            var nameOfFunction = tokenStream.Take( TokenType.Word );

            var arguments = GetArgumentList();

            return new MethodInvoke( nameOfFunction, arguments );
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

        private bool IsValidMethodReturnType()
        {
            switch( tokenStream.Current.TokenType )
            {
                case TokenType.Void:
                case TokenType.Word:
                case TokenType.Int:
                case TokenType.String:
                case TokenType.Var:
                case TokenType.Boolean:
                    return true;
            }
            return false;
        }

        private bool IsValidVariableName( Token item )
        {
            switch( item.TokenType )
            {
                case TokenType.Word:
                    return true;
            }
            return false;
        }

    }
}
