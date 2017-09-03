using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class InterpretorException : CompilerException
    {
        public InterpretorException() : base() { }
        public InterpretorException( string message ) : base(message) { }
        public InterpretorException( string message, System.Exception inner ) : base(message, inner) { }

    }
}
