using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries
{
    public abstract class AbstractClass
    {
        protected bool isStatic = true;

        public bool IsStatic()
        {
            return this.isStatic;
        }

        public abstract string GetName();
        public abstract List<MethodDef> GetMethods();
        public abstract dynamic InvokeMethod( MethodDef definition, object arguments );
    }
}
