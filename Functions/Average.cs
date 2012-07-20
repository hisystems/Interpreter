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
	/// Accepts one argument of type Array containing objects of type Number.
	/// </summary>
    public class Average : Function
    {
        public override string Name
        {
            get
            {
                return "AVG";
            }
        }

		public override Literal Execute(IConstruct[] arguments)
        {
			base.EnsureArgumentCountIs(arguments, 1);

            return new Number(base.GetTransformedArgumentDecimalArray(arguments, argumentIndex:0).Average());
        }
    }
}
