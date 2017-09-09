using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public class MethodDeclr : FunctionDeclr
    {
        public Token AccessModifier;

        public MethodDeclr( Token accessModifier, Token name, Token returnType, List<AST> args, AST body ) : base( name, returnType, args, body )
        {
            AccessModifier = accessModifier;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.MethodDeclr; }
        }
    }
}
