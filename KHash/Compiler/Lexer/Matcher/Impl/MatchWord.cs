using KHash.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KHash.Compiler.Lexer.Matcher.Impl
{
    public class MatchWord : BaseMatcher
    {
        private List<MatchKeyword> SpecialCharacters { get; set; }
        public MatchWord( IEnumerable<IMatcher> keywordMatchers )
        {
            SpecialCharacters = keywordMatchers.Select( i => i as MatchKeyword ).Where( i => i != null ).ToList();
        }

        protected override Token IsMatchImpl( Tokenizer tokenizer )
        {
            String current = null;

            while( !tokenizer.End() && !String.IsNullOrWhiteSpace( tokenizer.Current ) && SpecialCharacters.All( m => m.Match != tokenizer.Current ) )
            {
                current += tokenizer.Current;
                tokenizer.Consume();
            }

            if( current == null )
            {
                return null;
            }

            // can't start a word with a special character
            if( SpecialCharacters.Any( c => current.StartsWith( c.Match ) ) )
            {
                throw new LexerInvalidSyntaxException( String.Format( "Cannot start a word with a special character {0}", current ) );
            }

            return new Token( TokenType.Word, current );
        }
    }
}
