using KHash.Core.Compiler.Interpretors;
using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Compiler.Scope;
using KHash.Core.Exceptions;
using KHash.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Text;
using ILibrary = KHash.Core.Libraries.ILibrary;

namespace KHash.Core
{
    public class Dispatcher
    {
        private Interpretor interpretor;

        public Dispatcher( Interpretor interpretor )
        {
            this.interpretor = interpretor;
        }
    
        public dynamic Invoke( ILibrary library, AbstractClass classDef, FunctionInvoke invoke )
        {
            MethodDef method = null;
            var definedMethods = classDef.GetMethods();
            foreach( MethodDef m in definedMethods )
            {
                if( m.Name == invoke.Name.TokenValue )
                {
                    method = m;
                }
            }

            //Cant find method in class
            if( method == null )
            {
                return null;
            }
            
            return library.InvokeClassMethod( classDef, method, ParseArgs( method, invoke.Arguments ) );
        }

        private List<object> ParseArgs( MethodDef methodDef, List<AST> invokingArgs )
        {
            List<object> args = new List<object>();
           
            for( int a = 0; a < methodDef.Arguments.Count; a++ )
            {
                MethodArg defArgument = methodDef.Arguments[a];
                bool found = false;

                if( invokingArgs.Count > a )
                {
                    var invokingArg = invokingArgs[a];
                    var executedValue = this.interpretor.Execute( invokingArg );

                    //Type check here invokingArg is expected match
                    
                    args.Add( executedValue );
                    found = true;
                }

                if( found == false && defArgument.Expected == true )
                {
                    //Throw error
                    throw new MethodException( String.Format( "Method {0} expected argument at position {1} to be of type {2} - {3}", methodDef.Name, a, defArgument.Type,defArgument.Description ) );
                }
            }



            return args;
        }
    }
}
