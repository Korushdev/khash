using KHash.Core.Compiler.TypeChecker;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries.StdLib.IO
{
    public class Directory : AbstractClass, IStdLib
    {
        const string Current = "Current";

        public override List<MethodDef> GetMethods()
        {
            return new List<MethodDef>()
            {
                new MethodDef()
                {
                    Name = Current,
                    ReturnType = TypeDef.String
                }
            };
        }

        public override string GetName()
        {
            return "Directory";
        }

        public override dynamic InvokeMethod( MethodDef definition, List<object> arguments )
        {
            switch( definition.Name )
            {
                case Current:
                    return System.IO.Directory.GetCurrentDirectory();
            }

            return null;
        }
    }
}
