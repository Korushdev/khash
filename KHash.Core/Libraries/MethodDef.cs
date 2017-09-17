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

        public MethodDef()
        {
            Arguments = new List<MethodArg>();
        }
    }

    public class MethodArg
    {
        public TypeDef Type { get; set; }
        public string Description { get; set; }
        public bool Expected { get; set; }
        public MethodArg( TypeDef t, string desc, bool expected = true )
        {
            this.Type = t;
            this.Description = desc;
            Expected = expected;
        }
    }
}
