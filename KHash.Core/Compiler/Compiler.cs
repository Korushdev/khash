using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser;
using KHash.Core.Environment;
using KHash.Core.Exceptions;
using System.Collections.Generic;
using System.Linq;
using Buffer = KHash.Core.Compiler.OutputBuffer.OutputBuffer;

namespace KHash.Core.Compiler
{
    public class Compiler
    {
        IEnvironment environment;
        OptionFactory optionFactory;
        Buffer outputBuffer;

        string rawLines = "";

        public Compiler( IEnvironment env, ref Buffer output )
        {
            optionFactory = Factory.GetOptionFactory();
            environment = env;
            outputBuffer = output;
            Initialize();
        }

        private void Initialize()
        {
            //IntializeRawCode();
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
                if( this.environment.GetIO().Exists( pathToIndex ) == true )
                {
                    rawLines = this.environment.GetIO().ReadToEnd( pathToIndex );
                }
            }
        }

        public void Process()
        {
            //Create tokens usingthe lexer
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
