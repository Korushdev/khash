﻿using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Scope
{
    public class Container
    {
        private Scope current;
        public Stack<Scope> Scopes = new Stack<Scope>();

        private Scope global = new Scope();

        public Scope StartScope()
        {
            Scope newScope = new Scope();
            current = newScope;
            Scopes.Push( newScope );

            return newScope;
        }

        public Scope StartScope( Scope scope )
        {
            current = scope;
            Scopes.Push( scope );

            return scope;
        }

        public Scope EndScope()
        {
            var removedscope = Scopes.Pop();
            current = Scopes.FirstOrDefault();
            return removedscope;
        }

        public Scope Current()
        {
            if( current == null )
            {
                current = global;
            }
            return current;
        }

        public void SetCurrent( Scope newCurrent )
        {
            current = newCurrent;
        }

        public Scope Global()
        {
            return Current();
        }

        public void Assign( AST ast, object value, Scope scope )
        {
            scope.Memory[ast.Token.TokenValue] = scope.CreateValue( value );
        }
    }

    public class Scope
    {
        public Scope Parent;
        public Dictionary<string, MemoryValue> Memory = new Dictionary<string, MemoryValue>();

        public object GetMemoryValue( AST ast )
        {
            return GetMemoryValue( ast.Token.TokenValue );
        }

        private object GetValueFromMemory( string key )
        {
            if( Memory.ContainsKey( key ) )
            {
                return Memory[key].Value;
            }else if( Parent is ClassScope )
            {
                if( Parent.Memory.ContainsKey( key ) )
                {
                    return Parent.Memory[key].Value;
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
            if( setToParent && Parent != null )
            {
                Parent.Memory[key] = CreateValue( value );
            }else
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
            }else
            {
                if( Memory.ContainsKey( key ) )
                {
                    Memory[key] = CreateValue( value );
                }
                else if( Parent is ClassScope )
                {
                    if( Parent.Memory.ContainsKey( key ) )
                    {
                        Parent.Memory[key] = CreateValue( value );
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
    }

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
                }else
                {
                    throw new VariableException( String.Format( "Invalid access modifier, cannot set {0}, possible types are private, protected, public", value ) );
                }
            }
        }
    }
}
