using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public class MagicMethodDeclr : AST
    {
        public List<AST> Arguments;
        public AST Body;

        public MagicMethodDeclr( Token type, List<AST> args, AST body ) : base( type )
        {
            Arguments = args;
            Body = body;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.MagicMethodDeclr; }
        }
    }
}
