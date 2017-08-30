using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Environment
{
    public enum OptionKey
    {
        //.ini filename - config can be found here
        KHASH_INI_FILENAME,
        //.ini directory - config folder\
        KHASH_INI_DIR,
        //KHash .exe filename
        KHASH_EXE_FILENAME,
        //KHash .exe directory
        KHASH_EXE_DIR,
        //index.khash
        KHASH_INDEX_FILENAME,
        //KHash default start path where index.khash can be found
        KHASH_BASE_PATH
    }
}
