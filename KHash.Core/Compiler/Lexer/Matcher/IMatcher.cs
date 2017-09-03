using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Lexer.Matcher
{
    public interface IMatcher
    {
        Token IsMatch( Tokenizer tokenizer );
        TokenType GetTokenType();
    }
}
