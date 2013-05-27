/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
 
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

      http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
 ___________________________________________________ */

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

