﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KHash.Compiler.Lexer.Matcher.Impl
{
    public class MatchNumber : BaseMatcher
    {
        protected override Token IsMatchImpl( Tokenizer tokenizer )
        {

            var leftOperand = GetIntegers( tokenizer );

            if( leftOperand != null )
            {
                if( tokenizer.Current == "." )
                {
                    tokenizer.Consume();

                    var rightOperand = GetIntegers( tokenizer );

                    // found a float
                    if( rightOperand != null )
                    {
                        return new Token( TokenType.Float, leftOperand + "." + rightOperand );
                    }
                }

                return new Token( TokenType.Int, leftOperand );
            }

            return null;
        }

        private String GetIntegers( Tokenizer tokenizer )
        {
            var regex = new Regex( "[0-9]" );

            String num = null;

            while( tokenizer.Current != null && regex.IsMatch( tokenizer.Current ) )
            {
                num += tokenizer.Current;
                tokenizer.Consume();
            }

            if( num != null )
            {
                return num;
            }

            return null;

        }
    }
}
