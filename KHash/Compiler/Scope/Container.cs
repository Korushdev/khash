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

        public void SetMemoryValue( string key, object value )
        {
            GetCurrentMemory().Values.Add( key, value );            
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
