using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class MethodException : InterpretorException
    {
        public MethodException() : base() { }
        public MethodException( string message ) : base(message) { }
        public MethodException( string message, System.Exception inner ) : base(message, inner) { }

    }
}
