using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Environment
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

        public static bool Setup( string[] args )
        {
            currentEnvironment = new ConsoleEnvironment();
            bool setupAllowContinue = currentEnvironment.Setup(args);

            optionFactory = new OptionFactory(currentEnvironment);

            return setupAllowContinue;
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
