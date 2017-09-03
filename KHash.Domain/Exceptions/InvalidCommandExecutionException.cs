using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Exceptions
{
    public class InvalidCommandExecutionException : EnvironmentInitializationException
    {
        public bool CatchAtProgram = false;
        public InvalidCommandExecutionException() : base() { }
        public InvalidCommandExecutionException( string message ) : base( message ) { }
        public InvalidCommandExecutionException( string message, bool catchAtProgram ) : base( message )
        {
            this.CatchAtProgram = catchAtProgram;
        }

        public InvalidCommandExecutionException( string message, System.Exception inner ) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected InvalidCommandExecutionException( System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        { }
    }
}
