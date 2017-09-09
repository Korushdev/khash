using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Exceptions;
using KHash.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser
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

    public partial class KHashParser
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
            var ast = tokenStream.Capture( ClassDecleration )
                                 .Or( () => tokenStream.Capture( FunctionDecleration ) );
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
                .Or( For )
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
        
        private AST.AST FunctionDecleration()
        {
            if( IsValidMethodReturnType() == false )
            {
                throw new Exception();
            }

            return ParseFunctionDecleration();
        }

        private AST.AST ParseFunctionDecleration()
        {
            var type = tokenStream.Take( tokenStream.Current.TokenType );
            tokenStream.Take( TokenType.Function );
            var functionName = tokenStream.Take( TokenType.Word );

            var arguments = GetArgumentList( );

            var body = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket );

            return new FunctionDeclr( functionName, type, arguments, body );
        }

        private List<AST.AST> GetArgumentList( bool isDecleration = true )
        {
            tokenStream.Take( TokenType.OpenParenth );

            List<AST.AST> args = new List<AST.AST>();
            while( tokenStream.Current.TokenType != TokenType.CloseParenth )
            {

                var argument = isDecleration ? VariableDeclarationAndAssignment(true) : InnerStatement();

                if( tokenStream.Current.TokenType == TokenType.Comma )
                {
                    tokenStream.Take( tokenStream.Current.TokenType );
                }
                args.Add( argument );
            }

            tokenStream.Take( TokenType.CloseParenth );
            return args;
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
        
        private AST.AST VariableDeclarationAndAssignment( bool throwErr = false )
        {
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

                return new VarDeclr( type, name, expr );
            }

            if( throwErr )
            {
                throw new Exception();
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
            switch( statement.AstType )
            {
                case AstTypes.ClassDeclr:
                case AstTypes.MethodDeclr:
                case AstTypes.FunctionDeclr:
                case AstTypes.MagicMethodDeclr:
                case AstTypes.While:
                case AstTypes.For:
                case AstTypes.Conditional:
                case AstTypes.Switch:
                    return false;
            }
            return true;
        }

        private AST.AST FunctionCallStatement()
        {
            return tokenStream.Capture( FunctionCall );
        }

        private AST.AST FunctionCall()
        {
            var nameOfFunction = tokenStream.Take( TokenType.Word );

            var arguments = GetArgumentList( false );

            return new FunctionInvoke( nameOfFunction, arguments );
        }

        private bool IsValidMethodReturnType()
        {
            switch( tokenStream.Current.TokenType )
            {
                case TokenType.Void:
                case TokenType.Word:
                case TokenType.Int:
                case TokenType.String:
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
