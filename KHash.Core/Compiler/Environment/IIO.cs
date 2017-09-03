﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Environment
{
    public interface IIO
    {
        bool Exists( string path );
        string ReadToEnd( string path );
        string ReadAllAsText( string path );

        string CombinePath( params string[] paths );
        string JoinPaths( bool isFileEnding = true, params string[] paths );
        string JoinPaths( params string[] paths );
        string GetDirectorySeperator();
    }
}
