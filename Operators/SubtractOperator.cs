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
    /// Subtracts one numeric value from another or the number of days from a datetime value, or the difference between two dates.
    /// Usage: 
    ///   numericValue - numericValue
    ///   dateTime - numericDays
    ///   dateTime - dateTime
    /// Examples:
    ///   1 - 2
    ///   #2000-01-01# - 1
    ///   #2000-01-02# - #2000-01-01# 
    /// </summary>
    public class SubtractOperator : Operator
    {
        public SubtractOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            var argument1Transformed = base.GetTransformedConstruct<Literal>(argument1);
            var argument2Transformed = base.GetTransformedConstruct<Literal>(argument2);

            if (argument1Transformed is Number && argument2Transformed is Number)
                return ((Number)argument1Transformed) - ((Number)argument2Transformed);
            else if (argument1Transformed is DateTime && argument2Transformed is TimeSpan)
                return ((DateTime)argument1Transformed) - ((TimeSpan)argument2Transformed);
            else if (argument1Transformed is DateTime && argument2Transformed is DateTime)
                return (((DateTime)argument1Transformed) - ((DateTime)argument2Transformed));
			else if (argument1Transformed is TimeSpan && argument2Transformed is TimeSpan)
				return ((TimeSpan)argument1Transformed) - ((TimeSpan)argument2Transformed);
            else
                throw new InvalidOperationException(String.Format("Subtract operator requires arguments of type Number, DateTime, or TimeSpan. Argument types are {0} {1}.", argument1Transformed.GetType().Name, argument2Transformed.GetType().Name));
        }

        public override string Token
        {
            get
            {
                return "-";
            }
        }
    }
}
