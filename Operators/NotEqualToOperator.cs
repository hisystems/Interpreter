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
    /// Compares two numeric, text, boolean or datetime values.
    /// Usage: 
    ///   numericValue &lt;&gt; numericValue
    ///   booleanValue &lt;&gt; booleanValue
    ///   text &lt;&gt; text
    ///   dateTime = dateTime
    /// Examples:
    ///   1 &lt;&gt; 2
    ///   true &lt;&gt; false
    ///   'a' &lt;&gt; 'b'
    ///   #2000-1-1# &lt;&gt; #2000-1-2#
    /// </summary>
    public class NotEqualToOperator : Operator
    {
        private EqualToOperator equalToOperator = new EqualToOperator();

        public NotEqualToOperator()
        {
        }

        internal override Literal Execute(IConstruct argument1, IConstruct argument2)
        {
            return !(Boolean)equalToOperator.Execute(argument1, argument2);
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
