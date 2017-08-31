using KHash.Compiler.Lexer;
using KHash.Compiler.Parser.AST;
using KHash.Compiler.Scope;
using KHash.Environment;
using KHash.Exceptions;
using KHash.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Compiler
{
    public class Interpretor
    {
        private OutputBuffer.OutputBuffer outputBuffer;
        private Container container;
        private OptionFactory optionFactory;

        public Interpretor( OutputBuffer.OutputBuffer buffer )
        {
            this.outputBuffer = buffer;
            optionFactory = Factory.GetOptionFactory();
        }

        public void Start( AST ast )
        {
            container = new Container();

            Execute( ast );
        }

        public dynamic Execute( AST ast, AST declerationType = null )
        {
            try
            {
                switch( ast.AstType )
                {
                    case AstTypes.ScopeDeclr:
                        ScopeDeclr( (ScopeDeclr)ast );
                        break;
                    case AstTypes.Send:
                        Send( (SendAST)ast );
                        break;
                    case AstTypes.Expression:
                        var ret = Expression( (Expr)ast, declerationType );
                        if( ret != null )
                        {
                            return ret;
                        }
                        break;
                    case AstTypes.VarDeclr:
                        VarDecleration( (VarDeclr)ast );
                        break;
                    case AstTypes.Conditional:
                        Condition( (Conditional)ast );
                        break;
                    case AstTypes.Switch:
                        Switch( (Switch)ast );
                        break;
                    case AstTypes.While:
                        While( (While)ast );
                        break;
                    case AstTypes.MethodDeclr:
                        MethodDecleration( (MethodDeclr)ast );
                        break;
                    case AstTypes.MethodInvoke:
                        var methodReturnVal = MethodInvoke( (MethodInvoke)ast );
                        if( methodReturnVal != null )
                        {
                            return methodReturnVal;
                        }
                        break;
                    case AstTypes.Return:
                        Return( (Return)ast );
                        break;

                }
            }catch( Exception e )
            {
                if( e is InterpretorException || e is ReturnValueException)
                {
                    throw e;
                }
            }
            return null;
        }

        private void ScopeDeclr( ScopeDeclr ast )
        {
            container.AddScope();
            ast.ScopedStatements.ForEach( statement => Execute( statement ) );
        }

        public void MethodDecleration( MethodDeclr ast )
        {
            container.SetMemoryValue( ast.Name.TokenValue, ast );
        }

        public dynamic MethodInvoke( MethodInvoke ast )
        {
            var current = container.Current;

            var value = container.GetMemoryValue( ast.Name.TokenValue );
            if( value != null && value is MethodDeclr )
            {
                MethodDeclr declaredMethod = (MethodDeclr)value;
               
                try
                {
                    Execute( declaredMethod.Body );
                }
                catch( ReturnValueException returnValueException )
                {
                    return returnValueException.Value;
                }
            }
            return null;
        }

        public dynamic Return( Return ast )
        {
            var returnVal = Execute( ast.ReturnExpression );

            throw new ReturnValueException( returnVal );
        }

        public void Condition( Conditional condition )
        {
            var result = Execute( condition.Expression );
            bool success = Convert.ToBoolean( result );
            if( success == true )
            {
                //Execute the body
                Execute( condition.Body );
            }else
            {
                //Execute else statement
            }
        }

        public void While( While condition )
        {
            int iteration = 0;
            int maxIteration = Convert.ToInt32( optionFactory.GetOption( OptionKey.KHASH_MAX_ITERATIONS ) );
            var expressionResult = Execute( condition.Expression );
            bool passes = Convert.ToBoolean( expressionResult  );
            
            while ( passes && 
                  ( maxIteration > 0 ? iteration <= maxIteration : true ))
            {
                Execute( condition.Body );
                iteration++;
            }
        }
        

        public void Switch( Switch switchAST )
        {
            var expressionResult = Execute( switchAST.Expression );

            Case caseToExecute = null;
            foreach( Case caseAST in switchAST.Cases )
            {
                var caseResult = Execute( caseAST.Expression );
                if( caseAST.IsCaseOf == true )
                {
                    if( caseResult is bool )
                    {
                        if( caseResult == true )
                        {
                            caseToExecute = caseAST;
                            break;
                        }
                    }
                }else
                {
                    if( caseResult == expressionResult )
                    {
                        caseToExecute = caseAST;
                        break;
                    }
                }
            }

            if( caseToExecute != null )
            {
                Execute( caseToExecute.Body );
            }
        }

        private void VarDecleration( VarDeclr ast )
        {
            var variableValue = ast.VariableValue;
            
            var value = Execute( variableValue, ast.DeclarationType );

            var symbol = ast.VariableName.Token.TokenValue;

            container.SetMemoryValue( symbol, TokenHelper.CastByString( ast.DeclarationType.Token.TokenValue, value ) );
        }

        private void Send( SendAST ast )
        {
            var expression = Execute( ast.Expression );
            outputBuffer.Append( expression );
        }

        private dynamic Expression( Expr ast, AST declerationType = null )
        {
            var lhs = ast.Left;
            var rhs = ast.Right;

            switch( ast.Token.TokenType )
            {
                case TokenType.QuotedString:
                    return ast.Token.TokenValue;
                case TokenType.Int:
                    return Convert.ToInt32( ast.Token.TokenValue );
                case TokenType.Float:
                    return Convert.ToDouble( ast.Token.TokenValue );
                case TokenType.Word:
                    return GetDeclaredWord( ast );
            }
            
            if( TokenHelper.IsOperator( ast.Token ) )
            {
                return ApplyOperator( ast, declerationType );
            }

            return null;
        }

        public object GetDeclaredWord( AST ast )
        {
            var item = ast.Token.TokenValue;

            var current = container.Current;

            var value = container.GetMemoryValue( item );
            if( value != null )
            {
                return value;
            }

            return null;
        }

        private object ApplyOperator( Expr ast, AST declerationType = null )
        {
            dynamic leftExec = Execute( ast.Left, declerationType );
            dynamic rightExec = Execute( ast.Right, declerationType );

            var leftValue = leftExec;
            var rightValue = rightExec;
            //string returnType = ast.

            switch( ast.Token.TokenType )
            {
                case TokenType.Match:
                    return leftValue == rightValue;
                case TokenType.NotMatch:
                    return leftValue != rightValue;
                case TokenType.Plus:
                    return leftValue + rightValue;
                case TokenType.Minus:
                    return leftValue - rightValue;
                case TokenType.Asterix:
                    return leftValue * rightValue;
                case TokenType.Slash:
                    if( leftValue == 0 || rightValue == 0 )
                    {
                        throw new ExpressionInterpretorException( "Division of 0 is not allowed" );
                    }
                    if( declerationType != null )
                    {
                        switch( declerationType.Token.TokenValue )
                        {
                            case "int":
                                return (int)leftValue / rightValue;
                            case "float":
                                return (float)leftValue / rightValue;
                            case "double":
                                return (double)leftValue / rightValue;
                            case "decimal":
                                return (decimal)leftValue / rightValue;
                                
                        }
                    }
                    return leftValue / rightValue;
                case TokenType.GreaterThan:
                    return leftValue > rightValue;
                case TokenType.LessThan:
                    return leftValue < rightValue;
                case TokenType.GreaterThanOrEqual:
                    return leftValue >= rightValue;
                case TokenType.LessThanOrEqual:
                    return leftValue <= rightValue;
            }

            return null;
        }
    }

    public class ReturnValueException : Exception
    {
        public object Value;
        public ReturnValueException( object val )
        {
            Value = val;
        }
    }
}
