//using KHash.Environment;
using KHash.Environment;
using KHash.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compile = KHash.Compiler.Compiler;
using Buffer = KHash.Compiler.OutputBuffer.OutputBuffer;

namespace KHash
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if( Factory.Setup( args ) == true )
                {
                    IEnvironment env = Factory.GetEnv();

                    Buffer outputBuffer = new Buffer();
                    Compile compiler = new Compile( env, ref outputBuffer );

                    compiler.Process();

                    object[] outputStrings = outputBuffer.FetchOutput();
                    env.HandleOutput( outputStrings );
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
