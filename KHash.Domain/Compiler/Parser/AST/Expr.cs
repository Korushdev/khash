using KHash.Domain.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Compiler.Parser.AST
{
    public class Expr : AST
    {
        public AST Left { get; private set; }

        public AST Right { get; private set; }

        public Expr( Token token )
            : base( token )
        {
        }

        public Expr( AST left, Token token, AST right )
            : base( token )
        {
            Left = left;
            Right = right;
        }
        
        public override AstTypes AstType
        {
            get { return AstTypes.Expression; }
        }

        public override string ToString()
        {
            if( Left == null && Right == null )
            {
                return Token.ToString();
            }

            return "(" + Left + " " + Token + " " + Right + ")";
        }
    }
}
