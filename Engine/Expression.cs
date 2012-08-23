/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
 ___________________________________________________ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Contains a parsed expression tree object and variables defined in the expression.
    /// </summary>
    public class Expression : IConstruct
    {
        /// <summary>
        /// Only used for error reporting.
        /// </summary>
        private string expression;

        /// <summary>
        /// The root construct of the expression tree.
        /// </summary>
        private IConstruct construct;

        /// <summary>
        /// All of the variables defined in the expression.
        /// </summary>
		private IDictionary<string, Variable> variables;

		/// <summary>
		/// Initializes a new instance of the <see cref="HiSystems.Interpreter.Expression"/> class.
		/// </summary>
		/// <param name='value'>
		/// Root construct in the expression tree. Calling IConstruct.Transform will return the actual value.
		/// </param>
        internal Expression(string originalExpression, IConstruct value, List<Variable> variables)
        {
			if (value == null)
				throw new ArgumentNullException();
            else if (String.IsNullOrEmpty(originalExpression))
                throw new ArgumentNullException();
                
			this.construct = value;
            this.expression = originalExpression;
            this.variables = TranslateVariabelsToDictionary(variables);
        }

		/// <summary>
		/// Returns the calculated value for the expression.
		/// Any variables should be set before calling this function.
		/// Will typically return a Number or Boolean literal value (depending on the type of expression).
		/// </summary>
		public Literal Execute()
		{
			return construct.Transform();
		}

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Casts the return value to type T (of type Literal)
        /// </summary>
        public T Execute<T>() where T : Literal
        {
            Literal result = construct.Transform();

            if (!(result is T))
                throw new InvalidCastException(String.Format("Return value from '{0}' is of type {1}, not of type {2}", this.expression, result.GetType().Name, typeof(T).Name));

            return (T)result;
        }

		/// <summary>
		/// Returns a dictionary containing all of the variables that were defined in the expression.
		/// If a variable is defined in multiple locations only one variable object is available in the dictionary.
		/// Variables are tokens/identifiers that could not be resolved to an operator or function name.
		/// Each variable should be assigned a value i.e: Variables["MyVariable"].Literal = (Number)1;
		/// </summary>
		public IDictionary<string, Variable> Variables 
		{
			get 
			{
				return this.variables;
			}
		}

        /// <summary>
        /// Converts a string value to an Expression that when Execute()'d will return the same Text literal value.
        /// </summary>
        public static implicit operator Expression(string stringLiteral)
        {
            return new Expression("\"" + stringLiteral + "\"", new Text(stringLiteral), new List<Variable>());
        }
        
        /// <summary>
        /// Converts a string value to an Expression that when Execute()'d will return the same Text literal value.
        /// </summary>
        public static implicit operator Expression(bool value)
        {
            return new Expression(value.ToString(), new Boolean(value), new List<Variable>());
        }

        /// Returns a distinct list of variables from the expression.
        /// </summary>
        private static IDictionary<string, Variable> TranslateVariabelsToDictionary(List<Variable> variables)
        {
            return variables.ToDictionary(variable => variable.Name, variable => variable);
        }

        Literal IConstruct.Transform()
        {
            return construct.Transform();
        }

        public override string ToString()
        {
            return expression;
        }
    }
}
