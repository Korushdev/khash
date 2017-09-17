using KHash.Core.Compiler.Lexer;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public enum AstTypes
    {
        Expression,
        FunctionDeclr,
        FunctionInvoke,
        ScopeDeclr,
        Send,
        VarDeclr,
        Conditional,
        Switch,
        Return,
        While,
        For,
        ClassDeclr,
        PropertyDeclr,
        MethodDeclr,
        MagicMethodDeclr,
        ClassInvoke,
        ClassRef
    }
    
    [Union( 0, typeof( ScopeDeclr ) )]
    [MessagePackObject]
    public abstract class AST
    {
        [Key(0)]
        public Token Token { get; set; }
        
        public AST( Token token )
        {
            Token = token;
        }
        
        [Key(2)]
        public abstract AstTypes AstType { get; }
    }
}