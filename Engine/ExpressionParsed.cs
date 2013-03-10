using System;
using System.Collections.Generic;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents an expression that has been parsed and can be executed immediately without compilation/parsing.
    /// </summary>
    internal class ExpressionParsed : Expression
    {
        /// <summary>
        /// The root construct of the expression tree.
        /// </summary>
        private IConstruct construct;
        
        /// <summary>
        /// All of the variables defined in the expression.
        /// </summary>
        private IDictionary<string, Variable> variables;

        internal ExpressionParsed(string expression, IConstruct value, List<Variable> variables)
            : base(expression)
        {
            if (value == null)
                throw new ArgumentNullException();
            
            this.construct = value;
            this.variables = TranslateVariablesToDictionary(variables);
        }

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Will typically return a Number or Boolean literal value (depending on the type of expression).
        /// </summary>
        public override Literal Execute()
        {
            return construct.Transform();
        }

        /// <summary>
        /// Returns a dictionary containing all of the variables that were defined in the expression.
        /// If a variable is defined in multiple locations only one variable object is available in the dictionary.
        /// Variables are tokens/identifiers that could not be resolved to an operator or function name.
        /// Each variable should be assigned a value i.e: Variables["MyVariable"].Literal = (Number)1;
        /// </summary>
        public override IDictionary<string, Variable> Variables
        {
            get 
            {
                return this.variables;
            }
        }
    }
}

