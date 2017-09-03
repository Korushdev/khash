using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class LexerInvalidSyntaxException : LexerException
    {
        public LexerInvalidSyntaxException() : base() { }
        public LexerInvalidSyntaxException( string message ) : base(message) { }
        public LexerInvalidSyntaxException( string message, System.Exception inner ) : base(message, inner) { }

     
    }
}
