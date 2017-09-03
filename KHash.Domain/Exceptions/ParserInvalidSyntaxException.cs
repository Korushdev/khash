using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Exceptions
{
    public class ParserInvalidSyntaxException : ParserException
    {
        public ParserInvalidSyntaxException() : base() { }
        public ParserInvalidSyntaxException( string message ) : base(message) { }
        public ParserInvalidSyntaxException( string message, System.Exception inner ) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ParserInvalidSyntaxException( System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        { }
    }
}
