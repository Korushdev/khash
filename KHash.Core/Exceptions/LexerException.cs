using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class LexerException : CompilerException
    {
        public LexerException() : base() { }
        public LexerException( string message ) : base(message) { }
        public LexerException( string message, System.Exception inner ) : base(message, inner) { }
        
    }
}
