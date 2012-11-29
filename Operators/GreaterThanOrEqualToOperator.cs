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
    /// Compares two numeric or datetime values.
    /// Usage: 
    ///   numericValue &gt;= numericValue
    ///   dateTime &gt;= dateTime
    /// Examples:
    ///   1 &gt;= 2
    ///   #2000-01-02# &gt;= #2000-01-01#
    /// </summary>
    public class GreaterThanOrEqualToOperator : Operator
    {
        public GreaterThanOrEqualToOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            var argument1Transformed = base.GetTransformedConstruct<Literal>(argument1);
            var argument2Transformed = base.GetTransformedConstruct<Literal>(argument2);

            if (argument1Transformed is Number && argument2Transformed is Number)
                return ((Number)argument1Transformed) >= ((Number)argument2Transformed);
            else if (argument1Transformed is DateTime && argument2Transformed is DateTime)
                return ((DateTime)argument1Transformed) >= ((DateTime)argument2Transformed);
            else
                throw new InvalidOperationException(String.Format("Greater than or equal to operator requires arguments of type Number or DateTime. Argument types are {0} {1}.", argument1Transformed.GetType().Name, argument2Transformed.GetType().Name));
        }

        public override string Token
        {
            get 
            {
                return ">=";
            }
        }
    }
}
