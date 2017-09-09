using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public class FunctionInvoke : AST
    {
        public Token Name { get; set; }
        public List<AST> Arguments { get; private set; }

        public FunctionInvoke( Token name, List<AST> args) : base( new Token( TokenType.Function ) )
        {
            Name = name;
            Arguments = args;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.FunctionInvoke; }
        }
    }
}
