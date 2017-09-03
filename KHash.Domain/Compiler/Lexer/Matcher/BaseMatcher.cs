using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Compiler.Lexer.Matcher
{
    public abstract class BaseMatcher : IMatcher
    {
        public TokenType TokenType { get; set; }

        public Token IsMatch( Tokenizer tokenizer )
        {
            if( tokenizer.End() )
            {
                return new Token( TokenType.EOF );
            }

            tokenizer.TakeSnapshot();

            var match = IsMatchImpl( tokenizer );

            if( match == null )
            {
                tokenizer.RollbackSnapshot();
            }
            else
            {
                tokenizer.CommitSnapshot();
            }

            return match;
        }
        public TokenType GetTokenType()
        {
            return TokenType;
        }
        protected abstract Token IsMatchImpl( Tokenizer tokenizer );
    }
}
