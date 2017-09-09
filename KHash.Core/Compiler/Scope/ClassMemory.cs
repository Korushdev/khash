using KHash.Core.Compiler.Parser.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Scope
{
    public class ClassScope : Scope
    {
        public string Type { get; set; }
        public ClassDeclr ClassAST { get; set; }
        
    }
}
