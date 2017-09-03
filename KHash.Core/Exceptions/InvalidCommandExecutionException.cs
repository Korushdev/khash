using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
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

    
    }
}
