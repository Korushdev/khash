using BinaryFormatter;
using KHash.Core.Compiler.Interpretors;
using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Environment;
using KHash.Core.Exceptions;
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

        public void Build()
        {
            List<Token> tokens = new Lexer.Lexer( rawLines ).Lex().ToList();            
            KHashParser parser = new KHashParser( tokens );
            var ast = parser.Parse();
            
            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryConverter converter = new BinaryConverter();

            var size = Marshal.SizeOf( ast );
            // Both managed and unmanaged buffers required.
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal( size );
            // Copy object byte-to-byte to unmanaged memory.
            Marshal.StructureToPtr( ast, ptr, false );
            // Copy data from unmanaged memory to managed buffer.
            Marshal.Copy( ptr, bytes, 0, size );
            // Release unmanaged memory.
            Marshal.FreeHGlobal( ptr );

            File.WriteAllBytes( "DataFile.dat", bytes );
        }

        public void Run()
        {
            Interpretor interpretor = new Interpretor( outputBuffer );

            byte[] byteArray = File.ReadAllBytes( "DataFile.dat" );
            BinaryConverter converter = new BinaryConverter();

            var ast = converter.Deserialize<AST>( byteArray );
            interpretor.Start( ast );
        }

        public void Process()
        {
            List<Token> tokens = new Lexer.Lexer( rawLines ).Lex().ToList();


            KHashParser parser = new KHashParser( tokens );
            var ast = parser.Parse();

            Interpretor interpretor = new Interpretor( outputBuffer );

            interpretor.Start( ast );

        }
    }
}
