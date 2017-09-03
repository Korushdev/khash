using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Helpers
{
    public static class FilePath
    {
        public static string JoinPaths(bool isFileEnding = true, params string[] paths)
        {
            return FilePath.JoinPaths(paths);
        }

        public static string JoinPaths( params string[] paths)
        {
            return Path.Combine(paths);
        }

        public static string GetDirectorySeperator()
        {
            return Path.DirectorySeparatorChar.ToString();
        }
    }
}
