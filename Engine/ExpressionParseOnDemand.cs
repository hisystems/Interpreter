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
    /// Represents an expression where compiling / parsing is deferred to when required.
    /// </summary>
    internal class ExpressionParseOnDemand : Expression
    {
        internal delegate IConstruct ParseToConstructDelegate(string expression, List<Variable> variables);

        /// <summary>
        /// The callback that will convert the string expression to a construct when required.
        /// </summary>
        private ParseToConstructDelegate parseToConstruct;

        /// <summary>
        /// The root construct of the expression tree.
        /// When null indicates that the expression has not been parsed.
        /// </summary>
        private IConstruct construct = null;
        
        /// <summary>
        /// All of the variables defined in the expression.
        /// </summary>
        private IDictionary<string, Variable> variables;

        public ExpressionParseOnDemand(ParseToConstructDelegate parseToConstruct, string expression)
            : base(expression)
        {
            if (parseToConstruct == null)
                throw new ArgumentNullException();

            this.parseToConstruct = parseToConstruct;
        }

        /// <summary>
        /// Returns the calculated value for the expression.
        /// Any variables should be set before calling this function.
        /// Will typically return a Number or Boolean literal value (depending on the type of expression).
        /// </summary>
        public override Literal Execute()
        {
            EnsureExpressionParsed();

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
                EnsureExpressionParsed();

                return this.variables;
            }
        }

        /// <summary>
        /// Parses the expression if it has not yet been parsed.
        /// </summary>
        private void EnsureExpressionParsed()
        {
            if (this.construct == null)
            {
                var variablesList = new List<Variable>();
                this.construct = this.parseToConstruct(base.Source, variablesList);
                this.variables = TranslateVariablesToDictionary(variablesList);
            }
        }
    }
}