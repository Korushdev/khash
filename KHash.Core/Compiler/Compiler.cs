using BinaryFormatter;
using KHash.Core.Compiler.Interpretors;
using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Environment;
using KHash.Core.Exceptions;
using KHash.Core.Libraries.StdLib;
using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Buffer = KHash.Core.Compiler.OutputBuffer.OutputBuffer;

namespace KHash.Core.Compiler
{
    public class Compiler
    {
        IEnvironment environment;
        OptionFactory optionFactory;
        Buffer outputBuffer;

        string rawLines = "";

        Registry registry;

        public Compiler( IEnvironment env, ref Buffer output )
        {
            optionFactory = Factory.GetOptionFactory();
            environment = env;
            outputBuffer = output;
            Initialize();
        }

        private void Initialize()
        {
            registry = new Registry();
            registry.Register( new StandardLibrary() );
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

        public bool Build()
        {
            AST ast = GetASTTree( rawLines );

            byte[] byteArray = MessagePackSerializer.Serialize( ast );

            this.environment.GetIO().WriteBytes( byteArray, "Build.dat" );

            return true;
        }

        public void Run()
        {
            byte[] byteArray = this.environment.GetIO().ReadAllAsBytes( "Build.dat" );
            var ast = MessagePackSerializer.Deserialize<AST>( byteArray );
            Interpret( ast );
        }

        public void Process()
        {
            string rawLines = @"
string someFileData = 'hello world';
File.Write('Something/bob.txt', someFileData );
";

            AST ast = GetASTTree( rawLines );
            Interpret( ast );
        }

        public void Interpret( AST ast )
        {
            Interpretor interpretor = new Interpretor( outputBuffer, registry );
            interpretor.Start( ast );
        }

        public AST GetASTTree( string lines )
        {
            List<Token> tokens = new Lexer.Lexer( lines ).Lex().ToList();
            KHashParser parser = new KHashParser( tokens );
            return parser.Parse();
        }
    }
}
