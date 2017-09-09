﻿using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public class FunctionDeclr : AST
    {
        public Token Name { get; set; }
        public Token ReturnType { get; set; }
        public List<AST> Arguments { get; private set; }
        public AST Body { get; set; }

        public FunctionDeclr( Token name, Token returnType, List<AST> args, AST body ) : base( new Token( TokenType.Function ) )
        {
            Name = name;
            ReturnType = returnType;
            Arguments = args;
            Body = body;
        }

        public override AstTypes AstType
        {
            get { return AstTypes.FunctionDeclr; }
        }
    }
}
