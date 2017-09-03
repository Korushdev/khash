using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Environment
{
    public enum Types
    {
        CONSOLE,
        WEB_CGI
    }

    public static class Factory
    {
        static IEnvironment currentEnvironment;
        static OptionFactory optionFactory;

        public static void Setup( IEnvironment env )
        {
            currentEnvironment = env;
            optionFactory = new OptionFactory( GetEnv() );
        }

        public static IEnvironment GetEnv()
        {
            return currentEnvironment;
        }

        public static bool IsA( Types _type )
        {
            return true;
        }

        public static OptionFactory GetOptionFactory()
        {
            return optionFactory;
        }
    }
}
