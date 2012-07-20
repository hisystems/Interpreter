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
    /// Represents a variable value that is resolved when first accessed.
    /// The value is also cached to improve further calls.
    /// </summary>
    public class Variable : IConstruct
    {
        private string name;
        private Literal literal;
		
        /// <summary>
        /// </summary>
        public Variable(string name)
			: this(name, null)
        {
        }

        /// <summary>
        /// </summary>
        public Variable(string name, Literal value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name");

            this.name = name;
            this.literal = value;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// The associated literal value that should be associated with this variable.
        /// This value should be set before the IConstruct.Transform() function is called.
        /// </summary>
		/// <remarks>
		/// Originally considered making this of type IConstruct so that a variable could be 
		/// set to a function or operation. However, at this stage there would be no advantage to doing so
		/// because the variable is set explicitly in code and therefore there is no advantage to passing
		/// it as function or operation expression.
		/// </remarks>
        public Literal Value
        {
            get
            {
                return this.literal;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();

            	this.literal = value;
            }
        }
		
		Literal IConstruct.Transform()
		{
			if (this.literal == null)
				throw new InvalidOperationException(String.Format("Variable {0} has not been set", this.name));

			return this.literal;
		}

        public override string ToString()
        {
			return this.name + ": " + this.literal.ToString();
        }
    }
}
