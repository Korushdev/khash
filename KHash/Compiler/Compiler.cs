using KHash.Compiler.Lexer;
using KHash.Compiler.Parser;
using KHash.Environment;
using KHash.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffer = KHash.Compiler.OutputBuffer.OutputBuffer;

namespace KHash.Compiler
{
    public class Compiler
    {
        IEnvironment environment;
        OptionFactory optionFactory;
        Buffer outputBuffer;

        string rawLines;
        StreamReader intialFile;

        public Compiler( IEnvironment env, ref Buffer output )
        {
            optionFactory = Factory.GetOptionFactory();
            environment = env;
            outputBuffer = output;
            Initialize();
        }

        private void Initialize()
        {
            IntializeRawCode();
        }

        private void IntializeRawCode()
        {
            try
            {
                rawLines = environment.GetExecutableString();
            }
            catch( InvalidCommandExecutionException e )
            {
                if( e.CatchAtProgram )
                {
                    throw e;
                }
                //Attempt to get string from index.khash
                string pathToIndex = optionFactory.GetIndexKhashFilePath();
                if( File.Exists( pathToIndex ) == false )
                {
                    throw new InvalidOptionException( "Cannot parse index file, file could not be found in:" + pathToIndex );
                }
                intialFile = new StreamReader( pathToIndex );
                rawLines = intialFile.ReadToEnd();
            }
        }

        public void Process()
        {
            //Create tokens using the lexer
            string rawLines = @"
int i = 4; 
int function bob()
{ 
    return i + 5; 
} 
send bob();
";

            List<Token> tokens = new Lexer.Lexer( rawLines ).Lex().ToList();
            
            KHashParser parser = new KHashParser( tokens );
            var ast = parser.Parse();

            Interpretor interpretor = new Interpretor( outputBuffer );

            interpretor.Start( ast );
        }
    }
}
