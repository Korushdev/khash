using KHash.Core.Exceptions;
using KHash.Core.Helpers;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Environment
{
    public class OptionFactory
    {
        IEnvironment environment;
        IDictionary<OptionKey, string> optionDict;

        public OptionFactory( IEnvironment env )
        {
            this.environment = env;
            SetDefaults();

            ParseClientSettings();
        }

        private void SetDefaults()
        {
            optionDict = new Dictionary<OptionKey, string>();
            IIO io = this.environment.GetIO();
            this.optionDict.Add( OptionKey.KHASH_INI_FILENAME, "KHash.ini" );
            this.optionDict.Add( OptionKey.KHASH_INI_DIR, io.JoinPaths( environment.GetExecutablePath(), "conf" ) );

            this.optionDict.Add( OptionKey.KHASH_EXE_FILENAME, "KHash.exe" );
            this.optionDict.Add( OptionKey.KHASH_EXE_DIR, io.JoinPaths( environment.GetExecutablePath() ) );

            this.optionDict.Add( OptionKey.KHASH_BASE_PATH, io.JoinPaths( environment.GetCurrentWorkingDirectory() ) );
            this.optionDict.Add( OptionKey.KHASH_INDEX_FILENAME, "index.khash" );

            this.optionDict.Add( OptionKey.KHASH_MAX_ITERATIONS, "100" );

        }

        private void ParseClientSettings()
        {
            string path = GetIniPath();
            if( this.environment.GetIO().Exists( path ) == false )
            {
                throw new InvalidOptionException( "Cannot parse ini, file could not be found in:" + path );
            }

            //int counter = 0;
            //string line;

            //OptionParser parser = new OptionParser( this );

            //// Read the file and iterate it line by line.
            //System.IO.StreamReader file = new System.IO.StreamReader( path );
            //while( ( line = file.ReadLine() ) != null )
            //{
            //    parser.ParseLine( line, counter );
            //    counter++;
            //}

            //file.Close();
        }

        public void SetOption( OptionKey key, string value )
        {
            var foundOptions = optionDict.Where( x => x.Key == key );
            if( foundOptions.Count() > 0 )
            {
                optionDict[key] = value;
            }
        }

        public string GetOption( OptionKey key )
        {
            return optionDict.Where( x => x.Key == key )
                .FirstOrDefault()
                .Value;
        }

        public string GetIniPath()
        {
            string fileName = GetOption( OptionKey.KHASH_INI_FILENAME );

            string dir = GetOption( OptionKey.KHASH_INI_DIR );
            return this.environment.GetIO().CombinePath( dir, fileName );
        }

        public string GetIndexKhashFilePath()
        {
            string fileName = GetOption( OptionKey.KHASH_INDEX_FILENAME );
            string dir = GetOption( OptionKey.KHASH_BASE_PATH );
            return this.environment.GetIO().CombinePath( dir, fileName );
        }
    }
}
