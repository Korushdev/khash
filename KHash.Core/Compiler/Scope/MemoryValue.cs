using KHash.Core.Compiler.Lexer;
using KHash.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Scope
{
    public class MemoryValue
    {
        public object Value { get; set; }
        public Type Type { get; set; }

        private TokenType accessModifier = TokenType.Public;
        public TokenType AccessModifier
        {
            get
            {
                return accessModifier;
            }
            set
            {
                if( value == TokenType.Private || value == TokenType.Protected || value == TokenType.Public )
                {
                    accessModifier = value;
                }
                else
                {
                    throw new VariableException( String.Format( "Invalid access modifier, cannot set {0}, possible types are private, protected, public", value ) );
                }
            }
        }
    }
}
