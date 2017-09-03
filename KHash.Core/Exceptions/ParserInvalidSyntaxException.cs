using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class ParserInvalidSyntaxException : ParserException
    {
        public ParserInvalidSyntaxException() : base() { }
        public ParserInvalidSyntaxException( string message ) : base(message) { }
        public ParserInvalidSyntaxException( string message, System.Exception inner ) : base(message, inner) { }

    }
}
