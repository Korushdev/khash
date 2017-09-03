using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class CompilerException : KHashException
    {
        public CompilerException() : base() { }
        public CompilerException( string message ) : base(message) { }
        public CompilerException( string message, System.Exception inner ) : base(message, inner) { }
        
    }
}
