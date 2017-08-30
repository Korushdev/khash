using KHash.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Compiler.Parser.AST
{
    public class Return : AST
    {
        public AST ReturnExpression { get; private set; }

        public Return() : base( new Token( TokenType.Return ) )
        {
        }

        public Return( AST returnExpression ) : base( new Token( TokenType.Return ) )
        {
            ReturnExpression = returnExpression;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.Return; }
        }
    }
}
