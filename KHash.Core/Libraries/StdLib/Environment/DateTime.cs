using KHash.Core.Compiler.TypeChecker;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries.StdLib.Environment
{
    public class DateTime : AbstractClass, IStdLib
    {
        const string Now = "Now";

        public override List<MethodDef> GetMethods()
        {
            return new List<MethodDef>()
            {
                new MethodDef()
                {
                    Name = Now,
                    ReturnType = TypeDef.String
                }
            };
        }

        public override string GetName()
        {
            return "DateTime";
        }

        public override dynamic InvokeMethod( MethodDef definition, List<object> arguments )
        {
            switch( definition.Name )
            {
                case Now:
                    return System.DateTime.Now.ToString();
            }

            return null;
        }
    }
}
