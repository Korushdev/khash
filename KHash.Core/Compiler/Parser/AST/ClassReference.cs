using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Parser.AST
{
    public class ClassReference : AST
    {
        public AST ClassInstance { get; set; }

        public List<AST> Deferences { get; set; }

        public ClassReference( AST classInstance, List<AST> deferences )
            : base( classInstance.Token )
        {
            ClassInstance = classInstance;
            Deferences = deferences;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.ClassRef; }
        }

        public override string ToString()
        {
            return "( " + ClassInstance + ")";
        }
    }
}
