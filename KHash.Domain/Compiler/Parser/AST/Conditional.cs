using KHash.Domain.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Compiler.Parser.AST
{
    public class Conditional : AST
    {
        public AST Expression;
        public ScopeDeclr Body;

        public Conditional( Token token, AST expr, ScopeDeclr scopeDeclr ) : base( new Token( TokenType.If ))
        {
            Expression = expr;
            Body = scopeDeclr;
        }

        public override AstTypes AstType
        {
            get
            {
                return AstTypes.Conditional;
            }
        }
    }
}
