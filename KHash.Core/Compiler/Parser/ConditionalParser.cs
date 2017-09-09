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
        private AST.AST For()
        {
            if( tokenStream.Current.TokenType == TokenType.For )
            {
                return tokenStream.Capture( ParseFor );
            }
            return null;
        }

        private AST.AST ParseFor()
        {
            tokenStream.Take( TokenType.For );
            tokenStream.Take( TokenType.OpenParenth );

            AST.AST initStatement = VariableDeclarationAndAssignment();
            tokenStream.Take( TokenType.SemiColon );
            AST.AST condition = Expression();
            tokenStream.Take( TokenType.SemiColon );
            AST.AST iterateExpr = Expression();

            tokenStream.Take( TokenType.CloseParenth );

            var body = GetStatementsInScope( TokenType.LBracket, TokenType.RBracket );
            return new For( initStatement, condition, iterateExpr, body );
        }

        private AST.AST While()
        {
            if( tokenStream.Current.TokenType == TokenType.While )
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
    }
}
