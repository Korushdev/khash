using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Compiler.Scope;
using KHash.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Text;
using ILibrary = KHash.Core.Libraries.ILibrary;

namespace KHash.Core
{
    public class Dispatcher
    {
        public dynamic Invoke( ILibrary library, AbstractClass classDef, FunctionInvoke invoke )
        {
            MethodDef method = null;
            foreach( MethodDef m in classDef.GetMethods() )
            {
                if( m.Name == invoke.Name.TokenValue )
                {
                    method = m;
                }
            }

            //Cant find method in class
            if( method == null )
            {

            }

            List<object> args = new List<object>();
                
            return library.InvokeClassMethod( classDef, method, args );
        }
    }
}
