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
    /// Logical And operator.
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
