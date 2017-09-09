using System;
using System.Collections.Generic;

namespace KHash.Scope
{
    public class Container
    {
        public Stack<Scope> Scopes = new Stack<Scope>();
        
        private Scope global = new Scope();
        private Scope current;
        
        public Scope Start()
        {
            Scope newScope = new Scope();
            Scopes.Push( newScope );
            current = newScope;
            return newScope;
        }

        public Scope End()
        {
            current = Scopes.Pop();
            return current;
        }

        public Scope Current()
        {
            return current ?? global;
        }
    }

    public class Scope
    {
        public Dictionary<string, MemoryValue> Memory = new Dictionary<string, MemoryValue>();
    }

    public class MemoryValue
    {
        public object Value { get; set; }
        public string Type { get; set; }
    }
}