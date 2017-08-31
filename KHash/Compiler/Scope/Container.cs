using KHash.Compiler.Parser.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Compiler.Scope
{
    public class Container : BaseScope
    {
        public BaseScope Current;

        public void AddScope()
        {
            Scopes.Add( new InnerScope() );
        }
        
        public object GetMemoryValue( string key )
        {
            var memoryDict = GetCurrentMemory().Values;
            if( memoryDict.ContainsKey( key ) )
            {
                return memoryDict[key];
            }
            return null;   
        }

        public object GetMemoryValue( AST ast )
        {
            return GetMemoryValue( ast.Token.TokenValue );
        }

        public void SetMemoryValue( string key, object value )
        {
            Memory current = GetCurrentMemory();

            current.Values[key] = value;
        }

        public void SetMemoryValue( AST key, object value )
        {
            SetMemoryValue( key.Token.TokenValue, value );
        }

        public Memory GetCurrentMemory()
        {
            if( Current == null )
            {
                return Memory;
            }
            else
            {
                return Current.Memory;
            }
        }

    }
}
