using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Compiler.Scope;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.OutputBuffer
{
    public class ClassOutputDumper
    {
        private ClassScope classScope;
        private List<string> lines;

        public ClassOutputDumper( ClassScope classScope )
        {
            this.classScope = classScope;

            BuildLines();
        }        

        public override string ToString()
        {

            string newLineDel = System.Environment.NewLine;

            string output = "";
            this.lines.ForEach( l => output += l + newLineDel );
            return output;
        }

        private void BuildLines()
        {
            this.lines = new List<string>();
            this.lines.Add( String.Format( "{0}<{0}>", classScope.ClassAST.Name.TokenValue ) );
            this.lines.Add( "(" );

            foreach( var statement in classScope.ClassAST.Body.ScopedStatements )
            {
                if( statement.AstType == Parser.AST.AstTypes.PropertyDeclr )
                {
                    PropertyDeclr prop = (PropertyDeclr)statement;
                    string name = prop.VariableName.Token.TokenValue;
                    object val = ( classScope.Memory.ContainsKey( name ) ) ? classScope.Memory[name].Value : prop.VariableValue.Token.TokenValue;
                    this.lines.Add( String.Format( "\t[{0}]<{1}> {2} => {3}", prop.AccessModifier.TokenValue, prop.DeclarationType.Token.TokenValue, name, val ) );
                }
            }

            this.lines.Add( ")" );
        }
    }
}