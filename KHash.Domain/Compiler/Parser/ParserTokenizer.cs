using KHash.Domain.Compiler.Lexer;
using KHash.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Domain.Compiler.Parser
{
    internal class Memo
    {
        public AST.AST Ast { get; set; }
        public int NextIndex { get; set; }
    }

    public class ParserTokenizer : BaseTokenizer<Token>
    {
        private Dictionary<int, Memo> CachedAst = new Dictionary<int, Memo>();

        public ParserTokenizer( List<Token> tokens ) : base( () => tokens )
        {

        }

        public bool IsMatch( TokenType type )
        {
            return Current.TokenType == type;
        }

        public AST.AST Capture( Func<AST.AST> ast )
        {
            if( Alt( ast ) )
            {
                return Get( ast );
            }

            return null;
        }

        public AST.AST Get( Func<AST.AST> getter )
        {
            Memo memo;
            if( !CachedAst.TryGetValue( Index, out memo ) )
            {
                return getter();
            }

            Index = memo.NextIndex;

            //Console.WriteLine("Returning type {0} from index {1} as memo", memo.Ast, Index);
            return memo.Ast;
        }

        public Boolean Alt( Func<AST.AST> action )
        {
            TakeSnapshot();

            Boolean found = false;

            try
            {
                var currentIndex = Index;

                var ast = action();

                if( ast != null )
                {
                    found = true;

                    CachedAst[currentIndex] = new Memo
                    {
                        Ast = ast,
                        NextIndex = Index
                    };
                }
            }
            catch
            {

            }

            RollbackSnapshot();

            return found;
        }

        public Token Take( TokenType type )
        {
            if( IsMatch( type ) )
            {
                var current = Current;

                Consume();

                return current;
            }

            throw new ParserInvalidSyntaxException(
                String.Format( "Invalid Syntax. Expecting {0} but got {1}",
                                type,
                                Current.TokenType ) );
        }

        public override Token Peek( int lookahead )
        {
            var peeker = base.Peek( lookahead );

            if( peeker == null )
            {
                return new Token( TokenType.EOF );
            }

            return peeker;
        }

        public override Token Current
        {
            get
            {
                var current = base.Current;
                if( current == null )
                {
                    return new Token( TokenType.EOF );
                }
                return current;
            }
        }
    }
}
