using KHash.Core.Environment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KHash.CLI
{
    public class IO : IIO
    {
        public bool Exists( string path )
        {
            return File.Exists( path );
        }

        public string ReadToEnd( string path )
        {
            return File.ReadAllText( path );
        }

        public string ReadAllAsText( string path )
        {
            return File.ReadAllText( path );
        }

        public string CombinePath( params string[] paths )
        {
            return Path.Combine( paths );
        }

        public string JoinPaths( bool isFileEnding = true, params string[] paths )
        {
            return JoinPaths( paths );
        }

        public string JoinPaths( params string[] paths )
        {
            return Path.Combine( paths );
        }

        public string GetDirectorySeperator()
        {
            return Path.DirectorySeparatorChar.ToString();
        }
    }
}
