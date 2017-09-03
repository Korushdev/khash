using KHash.Domain.Environment;
using KHash.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compile = KHash.Domain.Compiler.Compiler;
using Buffer = KHash.Domain.Compiler.OutputBuffer.OutputBuffer;
using KHash.CLI.Environment;

namespace KHash.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IEnvironment consoleEnvironment = new ConsoleEnvironment();
                bool setupAllowContinue = consoleEnvironment.Setup( args );

                if( setupAllowContinue )
                {
                    Factory.Setup( consoleEnvironment );

                    Buffer outputBuffer = new Buffer();
                    Compile compiler = new Compile( consoleEnvironment, ref outputBuffer );

                    compiler.Process();

                    object[] outputStrings = outputBuffer.FetchOutput();
                    consoleEnvironment.HandleOutput( outputStrings );
                }
            }
            catch( Exception exception )
            {
                string errorType = exception.GetType().ToString();
                Console.WriteLine( errorType + " occured occured: " + exception.Message );
            }

            while( true ) { }
        }
    }
}
