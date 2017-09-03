using KHash.Core.Compiler.Parser.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Scope
{
    public class Container : BaseScope
    {
        public InnerScope Global;
        public InnerScope Current;

        public Container()
        {
            Global = new InnerScope();
            Current = Global;
        }

        public void StartScope()
        {
            InnerScope scope = new InnerScope();
            Current = scope;
            Scopes.Add( scope );
        }

        public void EndScope()
        {
            int currentIndex = Scopes.IndexOf( Current );

            Current = null;
            Scopes.RemoveAt( currentIndex );

            //Reset Current scope to the previous scope in the Scopes List
            if( currentIndex != 0 )
            {
                int resetIndex = currentIndex - 1;
                Current = Scopes[resetIndex];
            }
        }

        public object GetMemoryValue( string key )
        {
            return GetValueFromMemory( GetCurrentMemory(), key );
        }

        public object GetGlobalMemoryValue( string key )
        {
            return GetValueFromMemory( Global.Memory, key );
        }

        public object GetMemoryValue( AST ast )
        {
            return GetMemoryValue( ast.Token.TokenValue );
        }

        private object GetValueFromMemory( Memory memory, string key )
        {
            if( memory.Values.ContainsKey( key ) )
            {
                return memory.Values[key];
            }
            return null;
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
            return Current.Memory;
        }

    }
}
