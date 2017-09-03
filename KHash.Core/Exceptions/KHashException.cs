using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Exceptions
{
    public class KHashException : Exception
    {
        public KHashException() : base() { }
        public KHashException( string message ) : base(message) { }
        public KHashException( string message, System.Exception inner ) : base(message, inner) { }

    }
}
