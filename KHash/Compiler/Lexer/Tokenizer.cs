using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Compiler.Lexer
{
    public class Tokenizer : BaseTokenizer<String>
    {
        public Tokenizer( String source )
            : base( () => source.ToCharArray().Select( i => i.ToString( CultureInfo.InvariantCulture ) ).ToList() )
        {

        }
    }
}
