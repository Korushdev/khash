using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Lexer.Matcher;
using KHash.Core.Compiler.Lexer.Matcher.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Spec
{
    public class Definition
    {
        public static partial class Types
        {
            public const string
                Int = "int",
                Float = "float",
                Double = "Double",
                Decimal = "Decimal",
                Bool = "bool",
                String = "string";
        }
    
        public static List<IMatcher> Keywords = new List<IMatcher>()
        {
            new MatchKeyword(TokenType.BeginCode, "<K#"),
            new MatchKeyword(TokenType.EndCode, "#>"),
            new MatchKeyword(TokenType.New, "new"),
            new MatchKeyword(TokenType.Class, "class"),
            new MatchKeyword(TokenType.Construct, "Init"),
            new MatchKeyword(TokenType.Destruct, "Destroy"),
            new MatchKeyword(TokenType.Private, "private"),
            new MatchKeyword(TokenType.Protected, "protected"),
            new MatchKeyword(TokenType.Public, "public"),
            new MatchKeyword(TokenType.Static, "static"),
            new MatchKeyword(TokenType.Send, "send"),
            new MatchKeyword(TokenType.If, "if"),
            new MatchKeyword(TokenType.Else, "else"),
            new MatchKeyword(TokenType.Function, "function"),
            new MatchKeyword(TokenType.Function, "new"),
            new MatchKeyword(TokenType.Function, "make"),
            new MatchKeyword(TokenType.Switch, "switch"),
            new MatchKeyword(TokenType.Case, "case"),
            new MatchKeyword(TokenType.CaseOf, "caseOf"),
            new MatchKeyword(TokenType.Break, "break"),
            new MatchKeyword(TokenType.Return, "return"),
            new MatchKeyword(TokenType.While, "while"),
            new MatchKeyword(TokenType.For, "for")
        };

        public static List<IMatcher> SpecialCharacters = new List<IMatcher>()
        {
                new MatchKeyword(TokenType.LBracket, "{"),
                new MatchKeyword(TokenType.RBracket, "}"),
                new MatchKeyword(TokenType.Increment, "++"),
                new MatchKeyword(TokenType.Plus, "+"),
                new MatchKeyword(TokenType.Decrement, "--"),
                new MatchKeyword(TokenType.Minus, "-"),
                new MatchKeyword(TokenType.NotMatch, "!="),
                new MatchKeyword(TokenType.Match, "=="),
                new MatchKeyword(TokenType.Equals, "="),
                new MatchKeyword(TokenType.Asterix, "*"),
                new MatchKeyword(TokenType.Not, "!"),
                new MatchKeyword(TokenType.OpenParenth, "("),
                new MatchKeyword(TokenType.CloseParenth, ")"),
                new MatchKeyword(TokenType.Slash, "/"),
                new MatchKeyword(TokenType.And, "&&"),
                new MatchKeyword(TokenType.Ampersand, "&"),
                new MatchKeyword(TokenType.GreaterThan, ">")
                {
                    AllowAsSubString = false,
                    SpecialCharacters = Keywords.Where( x => x.GetTokenType() == TokenType.EndCode ).Select( i => i as MatchKeyword ).ToList()
                },
                new MatchKeyword(TokenType.LessThan, "<")
                {
                    AllowAsSubString = false,
                    SpecialCharacters = Keywords.Where( x => x.GetTokenType() == TokenType.BeginCode ).Select( i => i as MatchKeyword ).ToList()
                },
                new MatchKeyword(TokenType.GreaterThanOrEqual, ">="),
                new MatchKeyword(TokenType.LessThanOrEqual, "<="),
                new MatchKeyword(TokenType.Or, "||"),
                new MatchKeyword(TokenType.Colon, ":"),
                new MatchKeyword(TokenType.SemiColon, ";"),
                new MatchKeyword(TokenType.Dot, "."),
                new MatchKeyword(TokenType.Comma, ",")
        };
    }
}
