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
            RegisterClass( new Date.DateTime() );
        }

        public dynamic InvokeClassMethod( AbstractClass classDef, MethodDef method, object arguments )
        {

            return classDef.InvokeMethod( method, arguments );
        }
    }
}
