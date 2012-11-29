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
    /// Returns the sum total of the numeric values from an array.
    /// Usage: Sum(array)
    /// Example: Sum(Array(1, 2, 3))
    /// </summary>
    public class Sum : Function
    {
        public override string Name
        {
            get
            {
                return "SUM";
            }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 1);
            
            return new Number(base.GetTransformedArgumentDecimalArray(arguments, argumentIndex:0).Sum());
        }
    }
}
