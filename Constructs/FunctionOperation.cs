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
    /// Performs the actual execution of a function, resolving and passing all of the function arguments.
    /// </summary>
    public class FunctionOperation : IConstruct
    {
        private Function function;
        private IConstruct[] arguments = null;

        public FunctionOperation(Function function, IConstruct[] arguments)
        {
            if (function == null)
                throw new ArgumentNullException();

            this.function = function;
            this.arguments = arguments;
        }
        
        Literal IConstruct.Transform()
        {
            // Translation of the arguments does not occur here - it occurs in the Function.
            // Sometimes it is necessary that the arguments are not translated, for example by functions that interpret variables as meaning something else.

            return function.Execute(this.arguments);
        }

        /// <summary>
        /// The function that will be executed when this construct is transformed.
        /// </summary>
        public Function Function
        {
            get
            {
                return this.function;
            }
        }

        /// <summary>
        /// The arguments supplied to the function
        /// </summary>
        public IConstruct[] Arguments
        {
            get
            {
                return this.arguments;
            }
        }

        public override string ToString()
        {
            return this.function.ToString();
        }
    }
}
