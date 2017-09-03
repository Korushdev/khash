using KHash.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Compiler.Parser.AST
{
    public enum AstTypes
    {
        Expression,
        MethodDeclr,
        MethodInvoke,
        ScopeDeclr,
        Send,
        VarDeclr,
        Conditional,
        Switch,
        Return,
        While,
        For
    }

    public abstract class AST
    {
        public Token Token { get; set; }

        public List<AST> Children { get; private set; }

        public AST( Token token )
        {
            Token = token;
            Children = new List<AST>();
        }

        public void AddChild( AST child )
        {
            if( child != null )
            {
                Children.Add( child );
            }
        }
        
        public abstract AstTypes AstType { get; }
    }
}