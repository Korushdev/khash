using KHash.Core.Compiler.Lexer.Matcher;
using KHash.Core.Compiler.Lexer.Matcher.Impl;
using KHash.Core.Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Lexer
{
    public class Lexer
    {

        private Tokenizer Tokenizer { get; set; }
        private List<IMatcher> Matchers { get; set; }

        public Lexer( string lines )
        {
            Tokenizer = new Tokenizer( lines );
            Matchers = InitializeMatchers();
        }

        public List<IMatcher> InitializeMatchers()
        {
            var matchers = new List<IMatcher>( 64 );
            
            var keywords = Definition.Keywords;
            var specialCharacters = Definition.SpecialCharacters;

            // give each keyword the list of possible delimiters and not allow them to be 
            // substrings of other words, i.e. token fun should not be found in string "function"
            keywords.ForEach( keyword =>
            {
                var current = ( keyword as MatchKeyword );
                current.AllowAsSubString = false;
                current.SpecialCharacters = specialCharacters.Select( i => i as MatchKeyword ).ToList();
            } );


            matchers.Add( new MatchString( MatchString.QUOTE ) );
            matchers.Add( new MatchString( MatchString.TIC ) );

            matchers.AddRange( specialCharacters );
            matchers.AddRange( keywords );

            matchers.AddRange( new List<IMatcher>
                                        {
                                            new MatchWhitespace(),
                                            new MatchNumber(),
                                            new MatchWord(specialCharacters)
                                        } );

            return matchers;
        }

        public IEnumerable<Token> Lex()
        {
            var current = Next();

            while( current != null && current.TokenType != TokenType.EOF )
            {
                // skip whitespace
                if( current.TokenType != TokenType.WhiteSpace )
                {
                    yield return current;
                }

                current = Next();
            }
        }

        public Token Next()
        {
            if( Tokenizer.End() )
            {
                return new Token( TokenType.EOF );
            }
            
            return
                 ( from match in Matchers
                   let token = match.IsMatch( Tokenizer )
                   where token != null
                   select token ).FirstOrDefault();
        }
    }
}
