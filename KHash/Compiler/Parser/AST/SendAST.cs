using KHash.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Compiler.Parser.AST
{
    public class SendAST : AST
    {
        public AST Expression { get; private set; }
        public SendAST( AST expression ) : base(new Token(TokenType.Send) )
        {
            Expression = expression;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.Send; }
        }
    }
}
