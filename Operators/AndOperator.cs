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
    /// Logical And operator.
    /// Usage: booleanValue AND booleanValue
    /// Example: true AND false
    /// </summary>
    public class AndOperator : Operator
    {
        public AndOperator()
        {
        }

        /// <summary>
        /// Non-zero arguments are considered true.
        /// </summary>
        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            return base.GetTransformedConstruct<Boolean>(argument1) && base.GetTransformedConstruct<Boolean>(argument2);
        }

        public override string Token
        {
            get
            {
                return "AND";
            }
        }
    }
}
