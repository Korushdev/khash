using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Environment
{
    public interface IEnvironment
    {
        bool Setup(string[] args);
        Types GetType();
        string GetExecutableString();
        void HandleOutput( object[] lines );

        string GetExecutablePath();
        string GetCurrentWorkingDirectory();
    }
}
