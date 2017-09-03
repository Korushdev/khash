using KHash.Core.Compiler.Lexer;

namespace KHash.Core.Compiler.Parser.AST
{
    public class For : AST
    {
        public AST InitStatement;
        public AST Condition;
        public AST IteratedExpression;
        public ScopeDeclr Body;

        public For( AST init, AST condition, AST iterateExpr, ScopeDeclr scopeDeclr ) : base( new Token( TokenType.For ))
        {
            InitStatement = init;
            Condition = condition;
            IteratedExpression = iterateExpr;
            Body = scopeDeclr;
        }

        public override AstTypes AstType
        {
            get
            {
                return AstTypes.For;
            }
        }
    }
}
