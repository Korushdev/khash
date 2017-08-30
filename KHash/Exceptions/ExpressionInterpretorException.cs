using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Exceptions
{
    public class ExpressionInterpretorException : InterpretorException
    {
        public ExpressionInterpretorException() : base() { }
        public ExpressionInterpretorException( string message ) : base(message) { }
        public ExpressionInterpretorException( string message, System.Exception inner ) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ExpressionInterpretorException( System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context )
        { }
    }
}
