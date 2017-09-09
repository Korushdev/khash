using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Helpers
{
    public static class TokenHelper
    {
        public static bool IsOperator( Token token )
        {
            switch( token.TokenType )
            {
                case TokenType.Equals:
                case TokenType.Match:
                case TokenType.NotMatch:
                case TokenType.Increment:
                case TokenType.Decrement:
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Asterix:
                case TokenType.GreaterThan:
                case TokenType.LessThan:
                case TokenType.GreaterThanOrEqual:
                case TokenType.LessThanOrEqual:
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Slash:
                    return true;
            }
            return false;
        }

        public static bool HasNoRightExpression( TokenType type )
        {
            switch( type )
            {
                case TokenType.Increment:
                case TokenType.Decrement:
                    return true;
            }
            return false;
        }

        public static bool HasNoLeftExpression( TokenType type )
        {
            return HasNoRightExpression( type );
        }
    }
}
