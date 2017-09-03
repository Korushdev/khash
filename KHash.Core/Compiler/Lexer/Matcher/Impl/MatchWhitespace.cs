﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Lexer.Matcher.Impl
{
    public class MatchWhitespace : BaseMatcher
    {
        protected override Token IsMatchImpl( Tokenizer tokenizer )
        {
            bool foundWhiteSpace = false;

            while( !tokenizer.End() && String.IsNullOrWhiteSpace( tokenizer.Current ) )
            {
                foundWhiteSpace = true;

                tokenizer.Consume();
            }

            if( foundWhiteSpace )
            {
                return new Token( TokenType.WhiteSpace );
            }

            return null;
        }
    }
}
