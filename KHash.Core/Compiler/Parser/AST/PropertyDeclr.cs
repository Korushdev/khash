using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Parser.AST
{
    public class PropertyDeclr : VarDeclr
    {
        public Token AccessModifier;

        public PropertyDeclr( Token accessModifier, Token declType, Token name, AST value = null )
            : base( declType, name, value )
        {
            AccessModifier = accessModifier;
        }
        
        public override AstTypes AstType
        {
            get { return AstTypes.PropertyDeclr; }
        }

        public override string ToString()
        {
            return String.Format( "Declare {0} as {1} with value {2}",
                                 VariableName, DeclarationType, VariableValue );
        }
    }
}
