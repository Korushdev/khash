using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Exceptions
{
    public class LexerInvalidSyntaxException : LexerException
    {
        public LexerInvalidSyntaxException() : base() { }
        public LexerInvalidSyntaxException( string message ) : base(message) { }
        public LexerInvalidSyntaxException( string message, System.Exception inner ) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected LexerInvalidSyntaxException( System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        { }
    }
}
