using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Scope
{
    public class Container
    {
        public Scope Current;
        
        public Container()
        {
            Scope global = new Scope( this, "Global" );
            Current = global;
        }

        public void Assign( AST ast, object value, Scope scope )
        {
            scope.Memory[ast.Token.TokenValue] = scope.CreateValue( value );
        }
    }
}
