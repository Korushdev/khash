using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Exceptions
{
    public class EnvironmentInitializationException : KHashException
    {
        public EnvironmentInitializationException() : base() { }
        public EnvironmentInitializationException( string message ) : base(message) { }
        public EnvironmentInitializationException( string message, System.Exception inner ) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected EnvironmentInitializationException( System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        { }
    }
}
