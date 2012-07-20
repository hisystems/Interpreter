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
	/// Accepts three arguments:
	///   argument 1: condition
	///   argument 2: true result
	///   argument 3: false result
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
			var trueResult = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 1);
			var falseResult = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 2);

			return condition ? trueResult : falseResult;
        }
    }
}
