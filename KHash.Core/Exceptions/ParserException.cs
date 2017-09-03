using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class ParserException : CompilerException
    {
        public ParserException() : base() { }
        public ParserException( string message ) : base(message) { }
        public ParserException( string message, System.Exception inner ) : base(message, inner) { }
        
    }
}
