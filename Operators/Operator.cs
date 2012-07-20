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
    /// Base class for all arithmetic / logical / equality operators.
    /// </summary>
    public abstract class Operator 
    {
        /// <summary>
        /// The unique token that indicates this operation.
		/// For special character tokens (non alpha-numeric characters) this can be at most 2 characters.
        /// For example '*' for multiply, or '/' for divide.
        /// </summary>
        public abstract string Token { get; }

		/// <summary>
		/// Should execute the operation and return the appropriate construct.
		/// </summary>
        internal abstract Literal Execute(IConstruct argument1, IConstruct argument2);
		
        /// <summary>
		/// Gets the construct and transforms/executes it and returns it as of type T.
		/// If the transformed result is not of type T then an exception is thrown.
		/// Minimise the use of this function because it will traverse and execute the entire expression tree if the construct represents an operation or function.
        /// </summary>
		protected T GetTransformedConstruct<T>(IConstruct construct) where T : Literal
		{
			var transformedConstruct = construct.Transform();

			return CastConstructToType<T>(transformedConstruct);
		}

		/// <summary>
		/// Throws an exception if the argument passed is not the expected type.
		/// </summary>
		private T CastConstructToType<T>(IConstruct construct)
		{
			if (!(construct is T))
				throw new InvalidOperationException(String.Format("{0} construct is not of type {1} and cannot be used with the {2} operator", construct.ToString(), typeof(T).Name, this.Token));

			return (T)construct;
		}
    }
}
