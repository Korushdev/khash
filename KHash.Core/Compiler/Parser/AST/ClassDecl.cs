using KHash.Core.Compiler.Lexer;

namespace KHash.Core.Compiler.Parser.AST
{
    public class ClassDeclr : AST
    {
        public Token Name;
        public ScopeDeclr Body;
        public MagicMethods MagicMethods = new MagicMethods();
        public bool IsStatic = false;

        public ClassDeclr( Token name, ScopeDeclr scopeDeclr, bool isStatic = false ) : base( new Token( TokenType.New ) )
        {
            Name = name;
            Body = scopeDeclr;
            IsStatic = isStatic;
        }

        public override AstTypes AstType
        {
            get
            {
                return AstTypes.ClassDeclr;
            }
        }
    }

    public class MagicMethods
    {
        public MagicMethodDeclr Constructor;
        public MagicMethodDeclr Destructor;
    }
}
