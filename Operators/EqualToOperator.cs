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
	/// Compares two numeric or boolean values.
    /// </summary>
    public class EqualToOperator : Operator
    {
        public EqualToOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
		{
			var argument1Transformed = base.GetTransformedConstruct<Literal>(argument1);
			var argument2Transformed = base.GetTransformedConstruct<Literal>(argument2);

			if (argument1Transformed is Number && argument2Transformed is Number)
				return new Boolean((decimal)((Number)argument1Transformed) == (decimal)((Number)argument2Transformed));
			else if (argument1Transformed is Boolean && argument2Transformed is Boolean)
				return new Boolean((bool)((Boolean)argument1Transformed) == (bool)((Boolean)argument2Transformed));
			else
			{
				// This will cause an exception to be thrown detailing that the construct cannot be converted to a number
				base.GetTransformedConstruct<Number>(argument1Transformed);
				base.GetTransformedConstruct<Number>(argument2Transformed);

				// should never call here - just here for compilation purposes regarding a return value 
				throw new InvalidOperationException();
			}
        }

        public override string Token
        {
            get 
            {
                return "=";
            }
        }
    }
}
