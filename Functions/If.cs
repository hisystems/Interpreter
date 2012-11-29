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
    /// Based on the condition / expression returns the true or false result.
    /// Usage: If(condition, trueResult, falseResult)
    /// Example: If(1 = 1, 'Yes', 'No') 
    /// </summary>
    public class If : Function
    {
        public override string Name
        {
            get
            {
                return "If";
            }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 3);

            var condition = base.GetTransformedArgument<Boolean>(arguments, argumentIndex: 0);

            if (condition)
                return base.GetTransformedArgument<Literal>(arguments, argumentIndex: 1);
            else 
                return base.GetTransformedArgument<Literal>(arguments, argumentIndex: 2);
        }
    }
}
