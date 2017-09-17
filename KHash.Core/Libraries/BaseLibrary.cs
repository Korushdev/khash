using KHash.Core.Compiler.Lexer;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries
{
    public class BaseLibrary<T>
    {
        protected IList<AbstractClass> classes = new List<AbstractClass>();
        
        public IList<AbstractClass> GetClasses()
        {
            return this.classes;
        }

        public void RegisterClass( AbstractClass item )
        {
            classes.Add( item );
        }

        public AbstractClass GetClass( Token name )
        {
            foreach( var classDef in classes )
            {
                if( classDef.GetName() == name.TokenValue )
                {
                    return classDef;
                }
            }
            return null;
        }
    }
}
