using KHash.Core.Compiler.Lexer;
using KHash.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Text;
using ILibrary = KHash.Core.Libraries.ILibrary;

namespace KHash.Core
{
    public class Registry
    {
        public List<ILibrary> Libraries = new List<ILibrary>();

        public void Register( ILibrary item )
        {
            Libraries.Add( item );
        }

        public Tuple<ILibrary, AbstractClass> GetClass( Token name )
        {
            foreach( ILibrary lib in Libraries )
            {
                AbstractClass classDef = lib.GetClass( name );
                if( classDef != null )
                {
                    return new Tuple<ILibrary, AbstractClass>( lib, classDef );
                }
            }
            return null;
        }
    }
}
