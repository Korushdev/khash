using KHash.Core.Compiler.Parser.AST;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Compiler.Scope
{
    public class Scope
    {
        private string name;
        private Container container;

        public Scope Parent;
        public List<Scope> Children = new List<Scope>();
        public Scope ParentClassScope;

        public Dictionary<string, MemoryValue> Memory = new Dictionary<string, MemoryValue>();
        
        public Scope( Container c, string name )
        {
            this.container = c;
            this.name = name;
        }

        public Scope StartScope( Scope scope = null, string name = "" )
        {
            Scope newScope = scope != null ? scope : new Scope( this.container, name );
            newScope.Parent = this;
            Children.Add( newScope );
            this.container.Current = newScope;
            return newScope;
        }

        public Scope EndScope()
        {
            if( this.Parent != null )
            {
                return this.Parent.EndScopeFromParent( this );
            }
            return null;
        }

        public Scope EndScopeFromParent( Scope child )
        {
            this.container.Current = this;
            this.Children.Remove( child );
            return child;
        }

        public object GetMemoryValue( AST ast )
        {
            return GetMemoryValue( ast.Token.TokenValue );
        }

        private object GetValueFromMemory( string key )
        {
            if( Memory.ContainsKey( key ) )
            {
                return Memory[key].Value;
            }
            else if( ParentClassScope is ClassScope )
            {
                if( ParentClassScope.Memory.ContainsKey( key ) )
                {
                    return ParentClassScope.Memory[key].Value;
                }
            }

            return null;
        }

        public object GetMemoryValue( string key )
        {
            return GetValueFromMemory( key );
        }

        public void SetMemoryValue( AST key, object value )
        {
            SetMemoryValue( key.Token.TokenValue, value );
        }

        public void SetMemoryValue( string key, object value, bool setToParent = false )
        {
            if( setToParent && ParentClassScope != null )
            {
                ParentClassScope.Memory[key] = CreateValue( value );
            }
            else
            {
                Memory[key] = CreateValue( value );
            }
        }

        public void ResetMemoryValue( AST key, object value )
        {
            ResetMemoryValue( key.Token.TokenValue, value );
        }

        public void ResetMemoryValue( string key, object value )
        {
            if( Memory.ContainsKey( key ) )
            {
                Memory[key] = CreateValue( value );
            }
            else
            {
                if( Memory.ContainsKey( key ) )
                {
                    Memory[key] = CreateValue( value );
                }
                else if( ParentClassScope is ClassScope )
                {
                    if( ParentClassScope.Memory.ContainsKey( key ) )
                    {
                        ParentClassScope.Memory[key] = CreateValue( value );
                    }
                }
            }

        }

        public MemoryValue CreateValue( object value )
        {
            if( value is MemoryValue )
            {
                return _CreateValue( (MemoryValue)value );
            }
            MemoryValue mem = new MemoryValue()
            {
                Type = value.GetType(),
                Value = value
            };
            return mem;
        }

        private MemoryValue _CreateValue( MemoryValue mem )
        {
            return mem;
        }

        public override string ToString()
        {
            return name != "" ? name : base.ToString();
        }
    }
}
