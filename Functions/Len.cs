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
    /// Accepts one argument of type string for which the length is returned.
    /// </summary>
    public class Len : Function
    {
        public override string Name
        {
            get
            {
                return "LEN";
            }
        }
		
        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIs(arguments, 1);
			
            return new Number(base.GetTransformedArgument<Text>(arguments, argumentIndex:0).Length);
        }
    }
}
