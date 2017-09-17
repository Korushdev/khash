using KHash.Core.Compiler.TypeChecker;
using System;
using System.Collections.Generic;
using System.Text;

namespace KHash.Core.Libraries.StdLib.IO
{
    public class File : AbstractClass, IStdLib
    {
        const string Exists = "Exists",
                     Move = "Move",
                     Write = "Write";

        public override List<MethodDef> GetMethods()
        {
            return new List<MethodDef>()
            {
                new MethodDef()
                {
                    Name = Exists,
                    ReturnType = TypeDef.Boolean,
                    Arguments = new List<MethodArg>
                    {
                        new MethodArg( TypeDef.String, "Path to file" )
                    }
                },
                new MethodDef()
                {
                    Name = Move,
                    ReturnType = TypeDef.Void,
                    Arguments = new List<MethodArg>
                    {
                        new MethodArg( TypeDef.String, "Move from" ),
                        new MethodArg( TypeDef.String, "Move to" )
                    }
                },
                new MethodDef()
                {
                    Name = Write,
                    ReturnType = TypeDef.Void,
                    Arguments = new List<MethodArg>
                    {
                        new MethodArg( TypeDef.String, "File path" ),
                        new MethodArg( TypeDef.String, "Contents" )
                    }
                }
            };
        }

        public override string GetName()
        {
            return "File";
        }

        public override dynamic InvokeMethod( MethodDef definition, List<object> arguments )
        {
            switch( definition.Name )
            {
                case Exists:
                    return System.IO.File.Exists( arguments[0].ToString() );
                case Move:
                    System.IO.File.Move( arguments[0].ToString(), arguments[1].ToString() );
                    break;
                case Write:
                    System.IO.File.WriteAllText( arguments[0].ToString(), arguments[1].ToString() );
                    break;
            }

            return null;
        }
    }
}
