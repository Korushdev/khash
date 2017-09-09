﻿using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public class VarDeclr : AST
    {
        public AST DeclarationType { get; protected set; }

        public AST VariableValue { get; protected set; }

        public AST VariableName { get; protected set; }

        protected VarDeclr( Token name ) : base( name )
        {
        }

        public VarDeclr( Token declType, Token name )
            : base( name )
        {
            DeclarationType = new Expr( declType );

            VariableName = new Expr( name );
        }

        public VarDeclr( Token declType, Token name, AST value )
            : base( name )
        {
            DeclarationType = new Expr( declType );

            VariableValue = value;

            VariableName = new Expr( name );
        }
        
        public override AstTypes AstType
        {
            get { return AstTypes.VarDeclr; }
        }

        public override string ToString()
        {
            return String.Format( "Declare {0} as {1} with value {2}",
                                 VariableName, DeclarationType, VariableValue );
        }
    }
}
