using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Lexer
{

    [MessagePackObject]
    public class Token
    {
        [Key(0)]
        public TokenType TokenType { get; private set; }
        
        [Key( 1 )]
        public String TokenValue { get; private set; }

        public Token( TokenType tokenType, String token )
        {
            TokenType = tokenType;
            TokenValue = token;
        }

        public Token( TokenType tokenType )
        {
            TokenValue = null;
            TokenType = tokenType;
        }

        public override string ToString()
        {
            return TokenType + ": " + TokenValue;
        }
    }
}
