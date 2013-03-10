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
    /// Represents an expression and the variables defined in the expression.
    /// This is the base class for an already compiled expression or a just-in-time compiled expression.
    /// </summary>
    public abstract class Expression : IConstruct
    {
        /// <summary>
        /// Only used for error reporting and debugging as the ToString return value.
        /// </summary>
        private string expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="HiSystems.Interpreter.Expression"/> class.
        /// </summary>
        /// <param name='value'>
        /// Root construct in the expression tree. Calling IConstruct.Transform will return the actual value.
        /// </param>
        internal Expression(string expression)
        {
            if (String.IsNullOrEmpty(expression))
                throw new ArgumentNullException();
                
            this.expression = expression;
        }

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Will typically return a Number or Boolean literal value (depending on the type of expression).
        /// </summary>
        public abstract Literal Execute();

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Casts the return value to type T (of type Literal)
        /// </summary>
        public T Execute<T>() where T : Literal
        {
            Literal result = this.Execute();

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
        public abstract IDictionary<string, Variable> Variables { get; }

        /// <summary>
        /// Converts a string value to an Expression that when Execute()'d will return the same Text literal value.
        /// </summary>
        public static implicit operator Expression(string stringLiteral)
        {
            return new ExpressionParsed("\"" + stringLiteral + "\"", new Text(stringLiteral), new List<Variable>());
        }
        
        /// <summary>
        /// Converts a bool value to an Expression that when Execute()'d will return the same bool literal value.
        /// </summary>
        public static implicit operator Expression(bool value)
        {
            return new ExpressionParsed(value.ToString(), new Boolean(value), new List<Variable>());
        }

        /// Returns a distinct list of variables from the expression.
        /// </summary>
        protected static IDictionary<string, Variable> TranslateVariablesToDictionary(List<Variable> variables)
        {
            return variables.ToDictionary(variable => variable.Name, variable => variable);
        }

        /// <summary>
        /// Allows for an expression to also be considered as a construct.
        /// Used when an expression references an identifier which is also a separate expression.
        /// Allowing an expression to reference a chain of expressions.
        /// </summary>
        Literal IConstruct.Transform()
        {
            return this.Execute();
        }

        public override string ToString()
        {
            return expression;
        }
    }
}
