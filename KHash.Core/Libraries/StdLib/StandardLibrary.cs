using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries.StdLib
{
    public class StandardLibrary : BaseLibrary<IStdLib>, ILibrary
    {
        public StandardLibrary()
        {
            Initialise();
        }

        public void Initialise()
        {
            RegisterClass( new Environment.DateTime() );

            RegisterClass( new IO.File() );
            RegisterClass( new IO.Directory() );
        }

        public dynamic InvokeClassMethod( AbstractClass classDef, MethodDef method, List<object> arguments )
        {

            return classDef.InvokeMethod( method, arguments );
        }
    }
}
