using KHash.CLI.Environment;
using KHash.Core.Environment;
using System;
using Compile = KHash.Core.Compiler.Compiler;
using Buffer = KHash.Core.Compiler.OutputBuffer.OutputBuffer;

namespace KHash.CLI
{
    class Program
    {
        static void Main( string[] args )
        {
            try
            {
                IEnvironment consoleEnvironment = new ConsoleEnvironment( new IO() );
                bool setupAllowContinue = consoleEnvironment.Setup( args );

                if( setupAllowContinue )
                {
                    Factory.Setup( consoleEnvironment );

                    Buffer outputBuffer = new Buffer();
                    Compile compiler = new Compile( consoleEnvironment, ref outputBuffer );

                    compiler.Process();
                    //if( compiler.Build() )
                    //{
                    //    compiler.Run();
                    //}

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

    public class Bob
    {
        public Bob()
        {

        }

        public static void Hello()
        {

        }
    }
}