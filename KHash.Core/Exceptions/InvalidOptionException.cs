using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class InvalidOptionException : EnvironmentInitializationException
    {
        public InvalidOptionException() : base() { }
        public InvalidOptionException( string message ) : base(message) { }
        public InvalidOptionException( string message, System.Exception inner ) : base(message, inner) { }

    }
}
