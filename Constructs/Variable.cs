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
        private IConstruct construct;
		
        /// <summary>
        /// </summary>
        public Variable(string name)
			: this(name, null)
        {
        }

        /// <summary>
        /// </summary>
        public Variable(string name, IConstruct value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name");

            this.name = name;
            this.construct = value;
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
        public IConstruct Value
        {
            get
            {
                return this.construct;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();

            	this.construct = value;
            }
        }
		
		Literal IConstruct.Transform()
		{
			if (this.construct == null)
				throw new InvalidOperationException(String.Format("Variable {0} has not been set", this.name));

			return this.construct.Transform();
		}

        public override string ToString()
        {
			return this.name + ": " + (this.construct == null ? String.Empty : this.construct.ToString());
        }
    }
}
