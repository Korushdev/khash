using KHash.Domain.Compiler.Lexer;

namespace KHash.Domain.Compiler.Parser.AST
{
    public class While : AST
    {
        public AST Expression;
        public ScopeDeclr Body;

        public While( Token token, AST expr, ScopeDeclr scopeDeclr ) : base( new Token( TokenType.While ))
        {
            Expression = expr;
            Body = scopeDeclr;
        }

        public override AstTypes AstType
        {
            get
            {
                return AstTypes.While;
            }
        }
    }
}
