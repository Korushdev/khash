using KHash.Core.Compiler.Lexer;
using System.Collections.Generic;

namespace KHash.Core.Compiler.Parser.AST
{
    public class ClassInvoke : AST
    {
        public Token Name { get; set; }
        public List<AST> Arguments { get; private set; }

        public ClassInvoke( Token name, List<AST> args ) : base( new Token( TokenType.New ) )
        {
            Name = name;
            Arguments = args;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.ClassInvoke; }
        }
    }
}
