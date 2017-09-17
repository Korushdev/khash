using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Scope
{
    public class ClassScope : Scope
    {
        public string Type { get; set; }
        public ClassDeclr ClassAST { get; set; }

        public bool IsInstantiated = false;
    }

    public class ClassLibraryScope : ClassScope
    {
        public AbstractClass ClassDef { get; set; }
        public ILibrary Library { get; set; }
    }
}
