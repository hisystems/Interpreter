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
using System.Linq;
using System.Text;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents a function that can be executed with a set of function arguments and return a function value.
    /// </summary>
    public abstract class Function
    {
        /// <summary>
        /// The unique name of the function that is used to identify the function in the token stream.
        /// </summary>
        public abstract string Name { get; }

        public abstract Literal Execute(IConstruct[] arguments);

        public Function()
        {
        }

        /// <summary>
        /// Throws an exception if the number of arguments supplied does not match the expected number of arguments.
        /// </summary>
        protected void EnsureArgumentCountIs(IConstruct[] arguments, int expectedArgumentCount)
        {
            if (expectedArgumentCount != arguments.Length)
                throw new InvalidOperationException(String.Format("{0} has been supplied {1} arguments, but expects {2} arguments", this.Name, arguments.Length, expectedArgumentCount));
        }
        
        /// <summary>
        /// Throws an exception if the number of arguments supplied is less then the minimum required number of arguments.
        /// Useful when the function can receive optional or a variable number of arguments.
        /// </summary>
        protected void EnsureArgumentCountIsAtLeast(IConstruct[] arguments, int minimumArgumentCount)
        {
            if (minimumArgumentCount > arguments.Length)
                throw new InvalidOperationException(String.Format("{0} has been supplied {1} arguments, but expects at least {2} arguments", this.Name, arguments.Length, minimumArgumentCount));
        }
        
        /// <summary>
        /// Throws an exception if the number of arguments supplied is less then the minimum required number of arguments.
        /// Or throws an exception if the number of arguments supplied is greater then the maximum allowable number of arguments.
        /// Useful when the function can receive optional or a variable number of arguments.
        /// </summary>
        protected void EnsureArgumentCountIsBetween(IConstruct[] arguments, int minimumArgumentCount, int maximumArgumentCount)
        {
            EnsureArgumentCountIsAtLeast(arguments, minimumArgumentCount);

            if (maximumArgumentCount < arguments.Length)
                throw new InvalidOperationException(String.Format("{0} has been supplied {1} arguments, but expects at most {2} arguments", this.Name, arguments.Length, maximumArgumentCount));
        }
        
        /// <summary>
        /// Returns an array of decimal values from a construct which must be of type Array.
        /// Minimise the use of this function because it will traverse and execute the entire expression tree if the construct represents an operation or function.
        /// <param name="argumentIndex">0 based index - error messages are reported as 1 based indexes</param>
        /// </summary>
        protected decimal[] GetTransformedArgumentDecimalArray(IConstruct[] arguments, int argumentIndex)  
        {
            return this.GetTransformedArgumentArray<Number>(arguments, argumentIndex).Select(number => (decimal)number).ToArray();
        }

        /// <summary>
        /// Returns an array of T values from a construct which must be of type Array.
        /// Minimise the use of this function because it will traverse and execute the entire expression tree if the construct represents an operation or function.
        /// <param name="argumentIndex">0 based index - error messages are reported as 1 based indexes</param>
        /// </summary>
        protected T[] GetTransformedArgumentArray<T>(IConstruct[] arguments, int argumentIndex) where T : Literal
        {
            var argument = GetArgument(arguments, argumentIndex);
            var transformedArgument = argument.Transform();

            var transformedArray = CastArgumentToType<Array>(transformedArgument, argumentIndex).Select(construct => construct.Transform());

            if (!transformedArray.All(item => item is T))
                throw new InvalidOperationException(String.Format("{0} argument {1} does not contain only {2} values and cannot be used with the {3} function", argument.ToString(), argumentIndex + 1, typeof(T).Name, this.Name));

            return transformedArray.Cast<T>().ToArray();
        }
        
        /// <summary>
        /// Gets the argument and transforms/executes it and returns it as of type T.
        /// If the transformed result is not of type T then an exception is thrown.
        /// Minimise the use of this function because it will traverse and execute the entire expression tree if the construct represents an operation or function.
        /// <param name="argumentIndex">0 based index - error messages are reported as 1 based indexes</param>
        /// </summary>
        protected T GetTransformedArgument<T>(IConstruct[] arguments, int argumentIndex) where T : Literal
        {
            var argument = GetArgument(arguments, argumentIndex);
            var transformedArgument = argument.Transform();

            return CastArgumentToType<T>(transformedArgument, argumentIndex);
        }

        /// <summary>
        /// Returns the argument (un-transformed).
        /// Throws an exception if there is no argument at the index.
        /// </summary>
        /// <param name="argumentIndex">0 based index - error messages are reported as 1 based indexes</param>
        private IConstruct GetArgument(IConstruct[] arguments, int argumentIndex) 
        {
            if (argumentIndex >= arguments.Length)
                throw new ArgumentException(String.Format("Function {0} is missing argument {1}.", this.Name, argumentIndex + 1));

            return arguments[argumentIndex];
        }

        /// <summary>
        /// Throws an exception if the argument passed, via the argument index is not the expected type.
        /// ArgumentIndex is only used for the error message and should match the index of the argument that is passed.
        /// </summary>
        private T CastArgumentToType<T>(IConstruct argument, int argumentIndex)
        {
            if (!(argument is T))
                throw new InvalidOperationException(String.Format("argument {1}{0} is not of type {2} and cannot be used with the {3} function", argument.ToString(), argumentIndex + 1, typeof(T).Name, this.Name));

            return (T)argument;
        }

        public override string ToString()
        {
            return this.Name + "()";
        }
    }
}
