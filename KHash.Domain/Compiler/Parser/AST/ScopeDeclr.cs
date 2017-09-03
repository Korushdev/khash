using KHash.Domain.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Compiler.Parser.AST
{
    public class ScopeDeclr : AST
    {
        public List<AST> ScopedStatements { get; private set; }

        public ScopeDeclr( List<AST> statements ) : base( new Token( TokenType.ScopeStart ) )
        {
            ScopedStatements = statements;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.ScopeDeclr; }
        }
    }
}
