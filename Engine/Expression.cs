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
        private IConstruct construct;
		private IDictionary<string, Variable> variables;

		/// <summary>
		/// Initializes a new instance of the <see cref="HiSystems.Interpreter.Expression"/> class.
		/// </summary>
		/// <param name='value'>
		/// Root construct in the expression tree. Calling IConstruct.Transform will return the actual value.
		/// </param>
        internal Expression(IConstruct value)
        {
			if (value == null)
				throw new ArgumentNullException();

			this.construct = value;
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
		/// Returns a dictionary containing all of the variables that were defined in the expression.
		/// If a variable is defined in multiple locations only one variable object is available in the dictionary.
		/// Variables are tokens/identifiers that could not be resolved to an operator or function name.
		/// Each variable should be assigned a value i.e: Variables["MyVariable"].Literal = (Number)1;
		/// </summary>
		public IDictionary<string, Variable> Variables 
		{
			get 
			{
				if (this.variables == null)
					this.variables = GetVariables(this.construct);

				return this.variables;
			}
		}
		
        /// Returns a distinct list of variables from the expression.
        /// </summary>
        private static IDictionary<string, Variable> GetVariables(IConstruct rootConstruct)
        {
            return GetVariablesList(rootConstruct)
                .ToDictionary(variable => variable.Name, variable => variable);
        }

        /// <summary>
        /// Returns a distinct list of variables from the expression.
        /// </summary>
        private static Variable[] GetVariablesList(IConstruct construct)
        {
            var variables = new List<Variable>();

            if (construct is Operation)
            {
                var operation = (Operation)construct;
                variables.AddRange(GetVariablesList(operation.LeftValue));
                variables.AddRange(GetVariablesList(operation.RightValue));
            }
            else if (construct is Variable)
            {
                variables.Add((Variable)construct);
            }
            else if (construct is FunctionOperation)
            {
                foreach (var functionArgument in ((FunctionOperation)construct).Arguments)
                	variables.AddRange(GetVariablesList(functionArgument));
            }

			return variables.ToArray();
		}

        Literal IConstruct.Transform()
        {
            return construct.Transform();
        }
    }
}
