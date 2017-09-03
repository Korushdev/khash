using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class VariableException : InterpretorException
    {
        public VariableException() : base() { }
        public VariableException( string message ) : base(message) { }
        public VariableException( string message, System.Exception inner ) : base(message, inner) { }
        
    }
}
