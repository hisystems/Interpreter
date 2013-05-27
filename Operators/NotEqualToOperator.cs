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
    /// Compares two numeric, text, boolean or datetime values.
    /// Usage: 
    ///   numericValue &lt;&gt; numericValue
    ///   booleanValue &lt;&gt; booleanValue
    ///   text &lt;&gt; text
    ///   dateTime = dateTime
    /// Examples:
    ///   1 &lt;&gt; 2
    ///   true &lt;&gt; false
    ///   'a' &lt;&gt; 'b'
    ///   #2000-1-1# &lt;&gt; #2000-1-2#
    /// </summary>
    public class NotEqualToOperator : Operator
    {
        private EqualToOperator equalToOperator = new EqualToOperator();

        public NotEqualToOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            return !(Boolean)equalToOperator.Execute(argument1, argument2);
        }

        public override string Token
        {
            get 
            {
                return "<>";
            }
        }
    }
}
