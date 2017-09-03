using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Lexer.Matcher.Impl
{
    public class MatchKeyword : BaseMatcher
    {
        public string Match { get; set; }



        /// <summary>
        /// If true then matching on { in a string like "{test" will match the first cahracter
        /// because it is not space delimited. If false it must be space or special character delimited
        /// </summary>
        public bool AllowAsSubString { get; set; }

        public List<MatchKeyword> SpecialCharacters { get; set; }

        public MatchKeyword( TokenType type, string match )
        {
            TokenType = type;
            Match = match;
            AllowAsSubString = true;
        }

        protected override Token IsMatchImpl( Tokenizer tokenizer )
        {
            if( TokenType == TokenType.Match )
            {

            }
            foreach( var character in Match )
            {
                if( tokenizer.Current == character.ToString(  ) )
                {
                    tokenizer.Consume();
                }
                else
                {
                    return null;
                }
            }

            bool found;

            if( !AllowAsSubString )
            {
                var next = tokenizer.Current;

                found = String.IsNullOrWhiteSpace( next ) || this.SpecialCharacters.Any( character => character.Match == next );
            }
            else
            {
                found = true;
            }

            if( found )
            {
                return new Token( TokenType, Match );
            }

            return null;
        }
    }
}
