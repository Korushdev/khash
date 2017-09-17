using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class ClassException : InterpretorException
    {
        public ClassException() : base() { }
        public ClassException( string message ) : base(message) { }
        public ClassException( string message, System.Exception inner ) : base(message, inner) { }
        
    }
}
