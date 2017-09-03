using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KHash.Core.Spec;
using KHash.Core.Exceptions;
using System.Text.RegularExpressions;

namespace KHash.Core.Environment
{
    public class OptionParser
    {
        OptionFactory context;

        public OptionParser( OptionFactory context )
        {
            this.context = context;
        }

        public void ParseLine( string _line, int lineNumber )
        {
            string line = CleanedLine( _line );
            if( line.Count() > 0 &&                 
                this.IsComment( line ) == false )
            {
                KeyValuePair<string, string> originalPair = this.GetPair( line, lineNumber );
                KeyValuePair<string, string> pair = this.UpperCasePairKey( originalPair );
                
                if( Enum.IsDefined( typeof( OptionKey ), pair.Key ) )
                {
                    OptionKey opt = (OptionKey)System.Enum.Parse( typeof( OptionKey ), pair.Key );
                    context.SetOption( opt, pair.Value );
                }else
                {
                    throw new InvalidOptionException( "Incorrectly formatted option found in : " + context.GetOption( OptionKey.KHASH_INI_FILENAME ) + " Line: " + lineNumber + " Value: " + line + " cannot be parsed" );
                }
            }
        }

        private string CleanedLine( string line )
        {
            var output = Regex.Replace( line, "^\\t", " " );
            return output.Trim();
        }

        private bool IsComment( string line )
        {
            return line.StartsWith( INI.COMMENT_BEGIN );
        }

        private KeyValuePair<string, string> GetPair( string line, int lineNumber )
        {
            List<string> parts = new List<string>();

            line.Split( new string[] { INI.VAR_EQUALS_OPERATOR }, StringSplitOptions.None )
                .ToList().ForEach( str => parts.Add( str.Trim() ) );

            if( parts.Count() != 2 )
            {
                throw new InvalidOptionException( "Incorrectly formatted option found in : " + context.GetOption( OptionKey.KHASH_INI_FILENAME ) + " Line: " + lineNumber + " Value: " + line  + " cannot be parsed" );
            }

            return new KeyValuePair<string, string>( parts[0], parts[1] );
        }

        private KeyValuePair<string, string> UpperCasePairKey( KeyValuePair<string, string> pair )
        {
            return new KeyValuePair<string, string>( pair.Key.ToUpper(), pair.Value );
        }
    }
}
