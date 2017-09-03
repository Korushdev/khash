using KHash.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Lexer.Matcher.Impl
{
    public class MatchString : BaseMatcher
    {
        public const string QUOTE = "\"";

        public const string TIC = "'";

        private String StringDelim { get; set; }

        public MatchString( String delim )
        {
            StringDelim = delim;
        }

        protected override Token IsMatchImpl( Tokenizer tokenizer )
        {
            var str = new StringBuilder();

            if( tokenizer.Current == StringDelim )
            {
                tokenizer.Consume();

                while( !tokenizer.End() && tokenizer.Current != StringDelim )
                {
                    str.Append( tokenizer.Current );
                    tokenizer.Consume();
                }

                if( tokenizer.Current == StringDelim )
                {
                    tokenizer.Consume();
                }
            }

            if( str.Length > 0 )
            {
                return new Token( TokenType.QuotedString, str.ToString() );
            }

            return null;
        }
    }
}
