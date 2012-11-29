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
    /// Returns the minimum numeric value from an array.
    /// Usage: Min(array)
    /// Example: Min(Array(1, 2, 3))
    /// </summary>
    public class Min : Function
    {
        public override string Name
        {
            get
            {
                return "Min";
            }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 1);
            
            return new Number(base.GetTransformedArgumentDecimalArray(arguments, argumentIndex:0).Min());
        }
    }
}
