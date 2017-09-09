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

        public void StreamReadByLine( string path, Action<string,int> callbackOnLine )
        {
            // Read the file and iterate it line by line.
            var stream = new FileStream( path, FileMode.Open );


            int counter = 0;
            string line;

            using( StreamReader file = new StreamReader( stream ) )
            {
                while( ( line = file.ReadLine() ) != null )
                {
                    callbackOnLine( line, counter );
                    counter++;
                }
            }
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
