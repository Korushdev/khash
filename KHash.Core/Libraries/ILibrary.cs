using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries
{
    public interface ILibrary
    {
        AbstractClass GetClass( Token name );
        dynamic InvokeClassMethod( AbstractClass classDef, MethodDef method, object arguments );
    }
}
