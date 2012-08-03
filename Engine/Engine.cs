/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
 ___________________________________________________ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace HiSystems.Interpreter
{
	/// <summary>
	/// The interpreter engine interprets single expressions that can contain;
	///   - variables
	///   - functions (custom functions also)
	///   - operators (mathematical / logical)
    ///   - literals (numbers, dates, strings)
	///   - parentheses - for precedence
	/// The Parse function will interpret the expression and return an Expression object.
	/// The expression can be supplied with the appropriate variables. 
	/// And then executed via the Expression.Execute() function.
	/// Example expressions:
	///  'IF(A > B, A, B)' 	-- requires calling Expression.Variables["A"] = (Number)1; Expression.Variables["B"] = (Number)2;
	///  'true or false'
	///  '(1 + 2) * 3 / 4'
    /// 
    /// See readme.md for further examples.
	/// </summary>
    public class Engine
    {
        private class OperatorAndPrecedence
        {
            public Operator Operation;
            public int Precedence;
        }
		
        private abstract class TranslatedToken
        {
        }

        private class ConstructToken : TranslatedToken
        {
            private IConstruct value;

            public ConstructToken(IConstruct value)
	        {
                if (value == null)
                    throw new ArgumentNullException();

                this.value = value;
	        }

            public IConstruct Value
            {
                get
                {
                    return this.value;
                }
            }

            public override string ToString()
            {
                return this.value.ToString();
            }
        }

        private class OperatorToken : TranslatedToken
        {
            private Operator operation;

            public OperatorToken(Operator operation)
	        {
                if (operation == null)
                    throw new ArgumentNullException();

                this.operation = operation;
	        }

            public Operator Value
            {
                get
                {
                    return this.operation;
                }
            }

            public override string ToString()
            {
                return this.operation.Token.ToString();
            }
        }

        private class LeftParenthesisToken : TranslatedToken
        {
            public override string ToString()
            {
                return "(";
            }
        }

        private class RightParenthesisToken : TranslatedToken
        {
            public override string ToString()
            {
                return ")";
            }
        }
		
		private class ReservedWord
		{
			/// <summary>
			/// The word that identifies the reserved word.
			/// </summary>
			public string Word;

			/// <summary>
			/// The construct that identifies the keyword.
			/// </summary>
			public IConstruct Construct;
		}

        /// <summary>
        /// </summary>
        /// <remarks>
        /// The precendence level is a relative level and is used to indicate the relative.
        /// A higher integer value means that it is performed first / before other operations.
        /// Operations with the same precedence level are executed left to right.
        /// Any new operations should be added to this list so that the tokens can be appropriately parsed
        /// by the parsing engine.
        /// </remarks>
        private static OperatorAndPrecedence[] allOperators = new [] {
            new OperatorAndPrecedence() { Operation = new MultiplyOperator(), Precedence = 6 },
            new OperatorAndPrecedence() { Operation = new DivideOperator(), Precedence = 6 },
            new OperatorAndPrecedence() { Operation = new AddOperator(), Precedence = 5 },
            new OperatorAndPrecedence() { Operation = new SubtractOperator(), Precedence = 5 },
            new OperatorAndPrecedence() { Operation = new LessThanOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new LessThanOrEqualToOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new GreaterThanOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new GreaterThanOrEqualToOperator(), Precedence = 4 },
            new OperatorAndPrecedence() { Operation = new EqualToOperator(), Precedence = 3 },
            new OperatorAndPrecedence() { Operation = new NotEqualToOperator(), Precedence = 3 },
            new OperatorAndPrecedence() { Operation = new AndOperator(), Precedence = 2 },
            new OperatorAndPrecedence() { Operation = new OrOperator(), Precedence = 1 }
        };

		/// <summary>
		/// Indicates which tokens of type Identifier need to be translated into special tokens.
		/// </summary>
		private static ReservedWord[] reservedWords = new [] 
		{
			new ReservedWord() { Word = "true", Construct = new Boolean(true) },
			new ReservedWord() { Word = "false", Construct = new Boolean(false) }
		};

        /// <summary>
        /// </summary>
        private static int parenthesisPrecendence;
        private List<Function> allFunctions = new List<Function>();
		
        static Engine() 
        {
            // Parentheses have higher precedence than all operations
            parenthesisPrecendence = allOperators
                .Select(item => item.Precedence)
                .Max() + 1;
        }

        /// <summary>
        /// Creates a new engine that can be used to parse expressions.
        /// </summary>
        public Engine()
        {
            Register(new Sum());
            Register(new Average());
            Register(new If());
        }

		/// <summary>
		/// Registers the function, so that the function can be utilised by the engine.
		/// Must be called before Engine.Parse() otherwise the function will not be recognised.
		/// </summary>
		public void Register(Function function)
		{
			allFunctions.Add(function);
		}
		
		/// <summary>
		/// Parses the expression and prepares it for execution.
		/// The returned Expression can then be populated with variables if necessary
		/// and then executed via Expression.Execute().
        public Expression Parse(string expression)
		{
            var variablesList = new List<Variable>();
            
			return new Expression(expression, ParseToConstruct(expression, variablesList), variablesList);
		}

		/// <summary>
		/// Parses the expression and prepares it for execution.
		/// </summary>
        private IConstruct ParseToConstruct(string expression, List<Variable> currentVariables)
        {
			return GetConstructFromTokens(Tokenizer.Parse(expression), currentVariables);
        }

		/// <summary>
		/// Creates the construct from a set of tokens.
		/// This is used to parse an entire expression and also an expression from a function's argument.
		/// </summary>
		private IConstruct GetConstructFromTokens(List<Token> tokens, List<Variable> currentVariables)
		{
            // Translate the tokens to meaningful tokens such as a variables, functions and operators
            // Unknown or unexpected tokens will cause an exception to be thrown
            var translatedTokens = TranslateTokens(tokens, currentVariables);

            // If there is only one item in the expression (i.e. a function or number and no operations)
            if (translatedTokens.Count == 1)
            {
                return ((ConstructToken)translatedTokens[0]).Value;
            }
            else
            {
                // Converts the tokens to the initial flat tree structure. 
                // The tree structure is flat (one level) and each Expression node is returned in the list.
                var expressions = TranslateTokensToOperations(translatedTokens);

                // Using the parentheses from the translated tokens determine the expression ordering
                SetExpressionPrecedenceFromParentheses(expressions.GetEnumerator(), translatedTokens.GetEnumerator(), depth: 0);

                // Set the ordering precedence based on the 
                SetExpressionPrecedenceFromOperators(expressions.GetEnumerator(), translatedTokens.GetEnumerator());

                // Enumerate through the ordered nodes and branch tree appropriately
                return TranslateToTreeUsingPrecedence(expressions);
            }
		}
 
        /// <summary>
        /// Translates the tokens into meaningful functions, operations and values.
        /// </summary>
        private List<TranslatedToken> TranslateTokens(List<Token> tokens, List<Variable> currentVariables)
        {
            var translatedTokens = new List<TranslatedToken>();
            var tokensEnum = new PeekableEnumerator<Token>(tokens);

            while (tokensEnum.MoveNext())
            {
                var token = tokensEnum.Current;

                switch (token.Type)
                {
                    case TokenType.Number:
                        translatedTokens.Add(new ConstructToken(Number.Parse(token.Value)));
                        break;
                    case TokenType.Identifier:
                        var operationForTokenIdentifier = allOperators
                            .Select(item => item.Operation)
                            .SingleOrDefault(item => item.Token.Equals(token.Value));

                        if (operationForTokenIdentifier != null)
                            translatedTokens.Add(new OperatorToken(operationForTokenIdentifier));
                        else
                            translatedTokens.Add(new ConstructToken(TranslateIdentifierToken(tokensEnum, currentVariables)));
                        break;
					case TokenType.LeftParenthesis:
                    	translatedTokens.Add(new LeftParenthesisToken());
						break;
					case TokenType.RightParenthesis:
                    	translatedTokens.Add(new RightParenthesisToken());
						break;
                    case TokenType.Text:
                        translatedTokens.Add(new ConstructToken(new Text(token.Value)));
                        break;
                    case TokenType.Other:
                        var operationForToken = allOperators
                            .Select(item => item.Operation)
                            .SingleOrDefault(item => item.Token.Equals(token.Value));

                        if (operationForToken != null)
                            translatedTokens.Add(new OperatorToken(operationForToken));
						else
                            throw new InvalidOperationException(token.Value + " in an unknown operation");

						break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return translatedTokens;
        }

        /// <summary>
		/// Translates the tokens to a set of operations. Each operation points to two child nodes of type ConstructToken.
        /// For example, 1 + 2 * 3 equates to 2 Operations: 
        ///    Operation1: LeftNode = 1, RightNode = 2. 
        ///    Operation2: LeftNode = 2, RightNode = 3,
        /// Expression 1 and 2 both link to the same Value (node 2). 
        /// This link is eventually broken and re-linked as the tree structure is created basd on the operation ordering.
        /// </summary>
        /// <remarks>
        /// Ignores processing of any other tokens that are not values or operations (parentheses for example) as 
        /// they are required for the ordering aspect of the translation process.
        /// </remarks>
        private static List<Operation> TranslateTokensToOperations(List<TranslatedToken> tokens)
        {
            var expectingOperation = false;
            var expressions = new List<Operation>();
            var currentExpression = new Operation();

            foreach (var token in tokens)
            {
                if (token is ConstructToken)     // function, variable or number
                {
                    if (expectingOperation)
                        throw new InvalidOperationException("Expecting operation not a value; " + token.ToString());

                    expectingOperation = true;  // on next iteration an operator is expected
                }
                else if (token is OperatorToken)
                {
                    if (!expectingOperation)
                        throw new InvalidOperationException("Expecting value not an operation; " + token.ToString());

                    expectingOperation = false; // on next iteration an operator is not expected
                }

                if (token is ConstructToken)     // function, variable or number
                {
                    if (currentExpression.LeftValue == null)
                        currentExpression.LeftValue = ((ConstructToken)token).Value;
                    else if (currentExpression.RightValue == null)
                    {
                        currentExpression.RightValue = ((ConstructToken)token).Value;
                        expressions.Add(currentExpression);
                        currentExpression = new Operation();
                        currentExpression.LeftValue = ((ConstructToken)token).Value;
                    }
                }
                else if (token is OperatorToken)
                {
                    currentExpression.Operator = ((OperatorToken)token).Value;
                }
            }

            return expressions;
        }

        /// <summary>
        /// Sets the precedence of the operators based on the parentheses defined in the token sequence.
        /// Each time a left parenthesis is found, the depth incremented and precedence set to a multiple (parenthesisPrecendence * depth).
        /// This is because within each parenthesis the precedence is higher, but there still needs to be precedence for the operators.
        /// The precedence for the operators is set on a subsequent pass.
        /// For example: 1 + (2 + 3 * 4), items 3 and 4 must be given higher precedence than the 2 + 3 because of the order of operations.
        /// So, if parenthesisPrecendence is 10, + is 1, * is 2 then precedence for the operators + + * would be 1,11,12. 
        /// For 1 + 2 + 3 * 4, then the precedence for the operators + + * would be 1,1,2. 
        /// </summary>
        private static void SetExpressionPrecedenceFromParentheses(IEnumerator<Operation> expressions, IEnumerator<TranslatedToken> translatedTokens, int depth)
        {
            while (translatedTokens.MoveNext())
            {
                var token = translatedTokens.Current;

                if (token is LeftParenthesisToken)
                {
                    SetExpressionPrecedenceFromParentheses(expressions, translatedTokens, depth + 1);
                }
                else if (token is OperatorToken)
                {
                    expressions.MoveNext();
                    // find the associated expression that is using the operator object instance
                    var expression = expressions.Current;
                    
                    if (expression.PrecedenceIsSet)
                        throw new InvalidOperationException("Expression precedence should not be set");

                    // Set the precedence explicitly considering this is the first pass: 'expression.PrecedenceSet == false'
                    expression.Precedence = depth * parenthesisPrecendence;
                }
                else if (token is RightParenthesisToken)
                {
                    break;
                }
            }

            if (depth > 0 && !(translatedTokens.Current is RightParenthesisToken))
                throw new InvalidOperationException("Missing ending right parenthesis token");
        }

        /// <summary>
        /// Sets the precedence of the operators based on the associated precedence.
        /// Assumes that previous passes (for the parentheses) have already set the precedence.
        /// Therefore, the value is incremented by the associated precedence. 
        /// </summary>
        /// <returns></returns>
        private static void SetExpressionPrecedenceFromOperators(IEnumerator<Operation> expressions, IEnumerator<TranslatedToken> translatedTokens)
        {
            while (translatedTokens.MoveNext())
            {
                var token = translatedTokens.Current;

                if (token is OperatorToken)
                {
                    expressions.MoveNext();
                    // find the associated expression that is using the operator object instance
                    var expression = expressions.Current;
                    expression.Precedence += allOperators.Where(item => item.Operation == ((OperatorToken)token).Value).Single().Precedence;
                }
            }
        }

        /// <summary>
        /// Builds the tree from a flattened tree with doubly linked nodes / items to a list a tree with one expression at the top. 
        /// Basically, high precedence items are processed first and pushed to the bottom of the tree
        /// so that they are executed first (their results feed into the other expressions, which feeds into other results etc.). 
        /// The tree is built based on the precedence value setup on previous passes.
        /// </summary>
        /// <param name="expressions"></param>
        /// <returns></returns>
        private static Operation TranslateToTreeUsingPrecedence(List<Operation> expressions)
        {
            var expressionsOrdered = expressions
                .Select((item, index) => new { Expression = item, LeftToRightIndex = index })
                .OrderByDescending(item => item.Expression.Precedence)
                .ThenBy(item => item.LeftToRightIndex)  // for expressions with the same precedence order from left to right
                .Select(item => item.Expression)        // remove the LeftToRightIndex now that it has been ordered correctly
                .GetEnumerator();

            while (expressionsOrdered.MoveNext() && expressions.Count > 1)
            {
                var orderedExpression = expressionsOrdered.Current;
                var orderedExpressionIndex = expressions.IndexOf(orderedExpression);

                // If there is an expression before this expression in the normal left to right index
                // then get it to point to this expression rather than the Value node that is shared with another expression.
                // Effectively, the orderedExpression is "pushed" to the bottom of the tree
                if (orderedExpressionIndex > 0)
                {
                    var previousExpression = expressions[orderedExpressionIndex - 1];
                    previousExpression.RightValue = orderedExpression;
                }

                // If there is an expression after this expression in the normal left to right index
                // then get it to point to this expression rather than the Value node that is shared with another expression.
                // Effectively, the orderedExpression is "pushed" to the bottom of the tree
                if (orderedExpressionIndex < expressions.Count - 1)
                {
                    var nextExpression = expressions[orderedExpressionIndex + 1];
                    nextExpression.LeftValue = orderedExpression;
                }

                // this expression has been pushed to the bottom of the tree (for an upside down tree)
                expressions.Remove(orderedExpression);
            }

            return expressions[0];
        }

        /// <summary>
        /// Enumerates through the tokens, searching for tokens XX,XX,XX where XX are arguments to the function and possibly a sub expression.
        /// When a ')' is found then the end of the arguments is presumed to have been found.
        /// </summary>
        /// <param name="tokensEnum">Expected to be currently pointing to the name of the function. The next token SHOULD be a '('</param>
		/// <remarks>
		/// NOTE: Currently, expressions as arguments is not supported. i.e. SUM(1 + 2, 3) will not work.
		/// </remarks>
        private IConstruct[] GetFunctionArguments(PeekableEnumerator<Token> tokensEnum, List<Variable> currentVariables)
        {
            var arguments = new List<IConstruct>();
            var functionName = tokensEnum.Current.Value;

            if (!(tokensEnum.MoveNext() && tokensEnum.Current.Type == TokenType.LeftParenthesis))
				throw new InvalidOperationException(String.Format("{0} arguments; first token should be '(' not '{1}'", functionName, tokensEnum.Current.Value));
            else if (tokensEnum.Current.Type == TokenType.LeftParenthesis && tokensEnum.CanPeek && tokensEnum.Peek.Type == TokenType.RightParenthesis)
                // No arguments were specified - empty parentheses were specified
                tokensEnum.MoveNext();;      // consume the left parenthesis token and point it to the right parenthesis token - i.e. the end of the function
            else
            {
				bool reachedEndOfArguments = false;

                while (!reachedEndOfArguments)
                {
					arguments.Add(GetConstructFromTokens(GetFunctionArgumentTokens(functionName, tokensEnum, currentVariables), currentVariables));

					// tokensEnum.Current will be the last token processed by GetFunctionArgumentTokens()
					if (tokensEnum.Current.Type == TokenType.RightParenthesis)
						reachedEndOfArguments = true;
                }
            }

            return arguments.ToArray();
        }

		/// <summary>
		/// Gets the function's next argument's tokens by traversing the tokens until the next , or ) is found (which is not within a function).
		/// Does not return the , or ) character that terminated the argument expression - it is also consumed.
		/// </summary>
		/// <param name="functionName">Only used in order to provide useful exceptions / errors.</param>
		/// <param name="tokensEnum">Should be pointing to the token that indicates the start of a function argument; either a ( or , character.</param>
        private static List<Token> GetFunctionArgumentTokens(string functionName, PeekableEnumerator<Token> tokensEnum, List<Variable> currentVariables)
		{
			var argumentTokens = new List<Token> ();

			int functionDepth = 0;
			bool reachedEndOfArgument = false;

			while (!reachedEndOfArgument && tokensEnum.MoveNext()) 
			{
				var token = tokensEnum.Current;

				// found the argument's terminating comma or right parenthesis
				if (functionDepth == 0 && (token.Type == TokenType.Comma || token.Type == TokenType.RightParenthesis))
					reachedEndOfArgument = true;
				else
				{
					argumentTokens.Add(token);

					if (token.Type == TokenType.LeftParenthesis)
						functionDepth++;
					else if (token.Type == TokenType.RightParenthesis)
						functionDepth--;
				}
			}

			if (argumentTokens.Count == 0)
				throw new InvalidOperationException(String.Format("{0} has an empty argument", functionName));
			else if (!reachedEndOfArgument)
				throw new InvalidOperationException(String.Format("{0} is missing a terminating argument character; ',' or ')'", functionName));

			return argumentTokens;
		}

        /// <summary>
        /// Translates an identifier as either a function, variable name or key word.
        /// A function will match a registered function name and have a left parenthesis token following it, otherwise it is a variable.
        /// </summary>
        private IConstruct TranslateIdentifierToken (PeekableEnumerator<Token> tokensEnum, List<Variable> currentVariables)
		{
			var identifierToken = tokensEnum.Current;

			var reservedWordForToken = reservedWords.SingleOrDefault(reserverWord => identifierToken == reserverWord.Word); 

			if (reservedWordForToken != null)
			{
                return reservedWordForToken.Construct;
			} 
			else
			{
	            var functionForToken = allFunctions.SingleOrDefault(aFunction => identifierToken == aFunction.Name);

	            if (functionForToken != null && tokensEnum.CanPeek && tokensEnum.Peek.Type == TokenType.LeftParenthesis)
	                return new FunctionOperation(functionForToken, GetFunctionArguments(tokensEnum, currentVariables));
	            else
	            {
	                // ensure there is only one Variable instance for the same variable name
	                var variable = currentVariables.SingleOrDefault(aVariable => identifierToken == aVariable.Name);

	                if (variable == null)
                    {
                        var newVariable = new Variable(tokensEnum.Current.Value);
                        currentVariables.Add(newVariable);
                        return newVariable;
                    }
	                else
	                    return variable;
	            }
			}
        }
    }
}
