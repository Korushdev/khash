using KHash.Domain.Compiler.Lexer;
using KHash.Domain.Compiler.Parser;
using KHash.Domain.Environment;
using KHash.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffer = KHash.Domain.Compiler.OutputBuffer.OutputBuffer;

namespace KHash.Domain.Compiler
{
    public class Compiler
    {
        IEnvironment environment;
        OptionFactory optionFactory;
        Buffer outputBuffer;

        string rawLines = "";
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
                if( File.Exists( pathToIndex ) == true )
                {
                    intialFile = new StreamReader( pathToIndex );
                    rawLines = intialFile.ReadToEnd();
                }
            }
        }

        public void Process()
        {
            //Create tokens using the lexer
            string rawLines = @"
string b = 'bob';

string function dog()
{
    return 123;
}

if( b == 'bob' )
{
send dog();
}
";

            List<Token> tokens = new Lexer.Lexer( rawLines ).Lex().ToList();
            
            KHashParser parser = new KHashParser( tokens );
            var ast = parser.Parse();

            Interpretor interpretor = new Interpretor( outputBuffer );

            interpretor.Start( ast );
        }
    }
}
