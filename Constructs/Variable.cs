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
    /// Represents a variable value that is resolved when first accessed.
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
