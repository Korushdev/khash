using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class EnvironmentInitializationException : KHashException
    {
        public EnvironmentInitializationException() : base() { }
        public EnvironmentInitializationException( string message ) : base(message) { }
        public EnvironmentInitializationException( string message, System.Exception inner ) : base(message, inner) { }
        
    }
}
