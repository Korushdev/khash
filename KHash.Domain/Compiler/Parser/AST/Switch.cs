using KHash.Domain.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Compiler.Parser.AST
{
    public class Switch : AST
    {
        public AST Expression;
        public List<Case> Cases;

        public Switch( AST expr, List<Case> cases ) : base( new Token( TokenType.Switch ))
        {
            Expression = expr;
            Cases = cases;
        }

        public override AstTypes AstType
        {
            get
            {
                return AstTypes.Switch;
            }
        }
    }
}
