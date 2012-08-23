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
    /// Usage: Max(array)
    /// </summary>
    public class Max : Function
    {
        public override string Name
        {
            get
            {
                return "MAX";
            }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 1);
            
            return new Number(base.GetTransformedArgumentDecimalArray(arguments, argumentIndex:0).Max());
        }
    }
}
