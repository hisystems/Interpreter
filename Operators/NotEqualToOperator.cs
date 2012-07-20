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
    public class NotEqualToOperator : Operator
    {
		private EqualToOperator equalToOperator = new EqualToOperator();

        public NotEqualToOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
			return new Boolean(!((bool)(Boolean)equalToOperator.Execute(argument1, argument2)));
        }

        public override string Token
        {
            get 
            {
                return "<>";
            }
        }
    }
}
