using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public class Case : AST
    {
        public AST Expression;
        public ScopeDeclr Body;
        public bool IsCaseOf;

        public Case( AST expr, ScopeDeclr body, bool isCaseOf = false ) 
            : base( new Token( isCaseOf ? TokenType.CaseOf : TokenType.Case ) )
        {
            Expression = expr;
            Body = body;
            IsCaseOf = isCaseOf;
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
