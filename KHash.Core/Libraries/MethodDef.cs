using KHash.Core.Compiler.TypeChecker;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries
{
    public class MethodDef
    {
        public string Name { get; set; }
        public List<MethodArg> Arguments { get; set; }

        public TypeDef ReturnType { get; set; }
    }

    public class MethodArg
    {
        public TypeDef Type { get; set; }
    }
}
