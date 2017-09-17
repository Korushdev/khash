using Force.DeepCloner;
using KHash.Core.Compiler.Lexer;
using KHash.Core.Compiler.Parser.AST;
using KHash.Core.Compiler.Scope;
using KHash.Core.Environment;
using KHash.Core.Exceptions;
using KHash.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHash.Core.Compiler.Interpretors
{
    public partial class Interpretor
    {
        private OutputBuffer.OutputBuffer outputBuffer;
        private Container container;
        private OptionFactory optionFactory;
        private Registry Libraries;
        private Dispatcher LibraryDispatcher = new Dispatcher();

        public Interpretor( OutputBuffer.OutputBuffer buffer, Registry libraries )
        {
            this.outputBuffer = buffer;
            optionFactory = Factory.GetOptionFactory();
            Libraries = libraries;
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
                    case AstTypes.For:
                        For( (For)ast );
                        break;
                    case AstTypes.Switch:
                        Switch( (Switch)ast );
                        break;
                    case AstTypes.While:
                        While( (While)ast );
                        break;
                    case AstTypes.FunctionDeclr:
                        FunctionDecleration( (FunctionDeclr)ast );
                        break;
                    case AstTypes.FunctionInvoke:
                        var functionReturnVal = FunctionInvoke( (FunctionInvoke)ast );
                        if( functionReturnVal != null )
                        {
                            return functionReturnVal;
                        }
                        break;
                    case AstTypes.ClassDeclr:
                        ClassDecleration( (ClassDeclr)ast );
                        break;
                    case AstTypes.ClassInvoke:
                        var instance = ClassInvoke( (ClassInvoke)ast );
                        if( instance != null )
                        {
                            return instance;
                        }
                        break;
                    case AstTypes.ClassRef:
                        var res = ClassReference( (ClassReference)ast );
                        if( res != null )
                        {
                            return res;
                        }
                        break;
                    case AstTypes.PropertyDeclr:
                        PropertyDecleration( (PropertyDeclr)ast );
                        break;
                    case AstTypes.MethodDeclr:
                        MethodDecleration( (MethodDeclr)ast );
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
            ast.ScopedStatements.ForEach( statement => Execute( statement ) );
        }

        public void FunctionDecleration( FunctionDeclr ast )
        {
            container.Current().SetMemoryValue( ast.Name.TokenValue, ast );
        }

        public dynamic FunctionInvoke( FunctionInvoke ast )
        {
            var functionName = ast.Name.TokenValue;
            var value = container.Current().GetMemoryValue( functionName );
            if( value != null && value is FunctionDeclr )
            {
                FunctionDeclr declaredFunction = (FunctionDeclr)value;

                SetArgumentsToMemory( declaredFunction.Name, declaredFunction.Arguments, ast.Arguments );

                try
                {
                    Execute( declaredFunction.Body );
                    HandleEndScopeForFunctionInvoking();
                }
                catch( ReturnValueException returnValueException )
                {
                    HandleEndScopeForFunctionInvoking();
                    var type = returnValueException.Value.GetType();
                    return returnValueException.Value;
                }
            }
            return null;
        }

        private void HandleEndScopeForFunctionInvoking()
        {
            var endedScope = container.EndScope();
            if( endedScope.Parent is ClassScope )
            {
                container.Scopes.Push( endedScope );
            }
        }

        private void SetArgumentsToMemory( Token functionName, List<AST> declaredArgument, List<AST> invokingArguments, Action startFunctionScope = null )
        {
            List<KeyValuePair<AST, object>> declaredArgValues = new List<KeyValuePair<AST, object>>();
            List<object> invokedArgValues = new List<object>();

            for( int c = 0; c < invokingArguments.Count(); c++ )
            {
                AST invokeArg = invokingArguments[c];
                var invokeArgVal = Execute( invokeArg );
                invokedArgValues.Add( invokeArgVal );
            }

            //Set scope start here
            if( startFunctionScope == null )
            {
                Scope.Scope current = container.Current();
                Scope.Scope newScope = container.StartScope();

                if( current.Parent != null && current.Parent is ClassScope )
                {
                    ClassScope classScope = (ClassScope)current.Parent;
                    newScope.Parent = classScope.DeepClone();
                }
            }
            else
            {
                startFunctionScope();
            }

            //Set default declared function args if they have been assigned a value
            foreach( VarDeclr functionArg in declaredArgument )
            {
                if( functionArg.VariableValue != null )
                {
                    var functionArgVal = Execute( functionArg.VariableValue );
                    var symbol = functionArg.Token.TokenValue;
                    container.Current().SetMemoryValue( symbol, TypeHelper.CastByString( functionArg.DeclarationType.Token.TokenValue, functionArgVal ) );
                }
            }

            //Set arg values to scope
            for( int c = 0; c < invokedArgValues.Count(); c++ )
            {
                var functionArg = (VarDeclr)declaredArgument.ElementAtOrDefault( c );
                if( functionArg != null )
                {
                    var symbol = functionArg.Token.TokenValue;
                    container.Current().SetMemoryValue( symbol, TypeHelper.CastByString( functionArg.DeclarationType.Token.TokenValue, invokedArgValues[c] ) );
                }
                else
                {
                    throw new MethodException( String.Format( "Function {0} expects {1} argument(s), you called {0} with {2} ", functionName.TokenValue, declaredArgument.Count(), invokingArguments.Count() ) );
                }
            }
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

        public void For( For condition )
        {
            int iteration = 0;
            int maxIteration = Convert.ToInt32( optionFactory.GetOption( OptionKey.KHASH_MAX_ITERATIONS ) );
            Execute( condition.InitStatement );

            for( container.Current().GetMemoryValue( condition.InitStatement ); Convert.ToBoolean( Execute( condition.Condition ) ); Execute( condition.IteratedExpression ) )
            {
                Execute( condition.Body );
                iteration++;
                if( maxIteration > 0 && iteration >= maxIteration )
                {
                    break;
                }
            }
        }

        public void While( While condition )
        {
            int iteration = 0;
            int maxIteration = Convert.ToInt32( optionFactory.GetOption( OptionKey.KHASH_MAX_ITERATIONS ) );
          
            while( Convert.ToBoolean( Execute( condition.Expression ) ) && 
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

            container.Current().SetMemoryValue( symbol, TypeHelper.CastByString( ast.DeclarationType.Token.TokenValue, value ) );
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
            string item = ast.Token.TokenValue;
            string lowerCaseWord = item.ToLower();
            switch( lowerCaseWord )
            {
                case "true":
                    return true;
                case "false":
                    return false;
            }

            var current = container.Current();
            var value = current.GetMemoryValue( item );
            if( value != null )
            {
                return value;
            }

            throw new VariableException( String.Format( "Variable {0} is undefined in current scope", item ) );
        }

        private object ApplyOperator( Expr ast, AST declerationType = null )
        {
            dynamic leftExec = Execute( ast.Left, declerationType );
            dynamic rightExec = Execute( ast.Right, declerationType );

            var leftValue = leftExec;
            var rightValue = rightExec;

            switch( ast.Token.TokenType )
            {
                case TokenType.Equals:
                    if( ast.Left.AstType == AstTypes.ClassRef )
                    {
                        var classRef = ( ast.Left as ClassReference );

                        var lastItem = classRef.Deferences.Last();

                        var fakeRef = new ClassReference( classRef.ClassInstance,
                                                         new List<AST>() { lastItem } );

                        Scope.Scope scope = ClassReference( fakeRef, true );
                        container.Assign( lastItem, rightValue, scope );
                    }
                    else
                    {
                        var current = container.Current();
                        current.ResetMemoryValue( ast.Left, rightValue );
                    }

                    break;
                case TokenType.Match:
                    return leftValue == rightValue;
                case TokenType.NotMatch:
                    return leftValue != rightValue;
                case TokenType.And:
                    return leftValue && rightValue;
                case TokenType.Increment:
                    //If (post increment ) i.e variable is to the left
                    if( ast.Left.Token.TokenType == TokenType.Word )
                    {
                        int incrIniValue = leftValue++;
                        var symbol = ast.Left.Token.TokenValue;
                        container.Current().SetMemoryValue( symbol, leftValue );
                        return incrIniValue;
                    }

                    //If (pre increment ) i.e variable is to the left
                    if( ast.Right != null && ast.Right.Token.TokenType == TokenType.Word )
                    {
                        ++rightValue;
                        var symbol = ast.Right.Token.TokenValue;
                        container.Current().SetMemoryValue( symbol, rightValue );
                        return rightValue;
                    }

                    return null;
                case TokenType.Decrement:
                    //If (post decrement ) i.e variable is to the left
                    if( ast.Left != null && ast.Left.Token.TokenType == TokenType.Word )
                    {
                        int decIniValue = leftValue--;
                        var symbol = ast.Left.Token.TokenValue;
                        container.Current().SetMemoryValue( symbol, leftValue );
                        return decIniValue;
                    }

                    //If (pre decrement ) i.e variable is to the left
                    if( ast.Right != null && ast.Right.Token.TokenType == TokenType.Word )
                    {
                        --rightValue;
                        var symbol = ast.Right.Token.TokenValue;
                        container.Current().SetMemoryValue( symbol, rightValue );
                        return rightValue;
                    }

                    return null;
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
