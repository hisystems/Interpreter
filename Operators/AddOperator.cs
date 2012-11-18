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
    /// Adds two numeric values, text or a date and numeric value (adds days).
    /// </summary>
    public class AddOperator : Operator
    {
        public AddOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            var argument1Transformed = base.GetTransformedConstruct<Literal>(argument1);
            var argument2Transformed = base.GetTransformedConstruct<Literal>(argument2);

            if (argument1Transformed is Number && argument2Transformed is Number)
                return ((Number)argument1Transformed) + ((Number)argument2Transformed);
            else if (argument1Transformed is DateTime && argument2Transformed is Number)
                return ((DateTime)argument1Transformed) + ((Number)argument2Transformed);
            else if (argument1Transformed is Text && argument2Transformed is Text)
                return ((Text)argument1Transformed) + ((Text)argument2Transformed);
            else
                throw new InvalidOperationException(String.Format("Add operator requires arguments of type Number, DateTime or Text. Argument types are {0} {1}.", argument1Transformed.GetType().Name, argument2Transformed.GetType().Name));
        }

        public override string Token
        {
            get 
            {
                return "+";
            }
        }
    }
}
