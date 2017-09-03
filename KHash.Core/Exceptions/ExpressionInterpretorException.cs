using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class ExpressionInterpretorException : InterpretorException
    {
        public ExpressionInterpretorException() : base() { }
        public ExpressionInterpretorException( string message ) : base(message) { }
        public ExpressionInterpretorException( string message, System.Exception inner ) : base(message, inner) { }

    }
}
