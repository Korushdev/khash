using KHash.Core.Compiler.Lexer;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{

    [MessagePackObject]
    public class ScopeDeclr : AST
    {
        [Key(2)]
        public List<AST> ScopedStatements { get; private set; }
        
        [SerializationConstructor]
        public ScopeDeclr( List<AST> statements ) : base( new Token( TokenType.ScopeStart ) )
        {
            ScopedStatements = statements;
        }

        [Key( 3 )]
        public override AstTypes AstType
        {
            get { return AstTypes.ScopeDeclr; }
        }
    }
}
