using KHash.Core.Compiler.Scope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.OutputBuffer
{
    public class OutputBuffer
    {
        List<object> outputLines = new List<object>();

        public OutputBuffer()
        {

        }

        public object[] FetchOutput()
        {
            return outputLines.ToArray();
        }

        public void Append( ClassScope outputLine )
        {
            outputLines.Add( new ClassOutputDumper( outputLine ) );
        }

        public void Append( object outputLine )
        {
            outputLines.Add( outputLine );
        }
    }
}
