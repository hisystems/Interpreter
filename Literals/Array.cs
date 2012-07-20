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
    /// Represents an array of constructs. 
	/// Potentially an array of literals, variables or functions.
    /// </summary>
    public class Array : Literal, IEnumerable<IConstruct>
    {
        private List<IConstruct> items = new List<IConstruct>();
		
        public Array(decimal[] values)
        {
            items.AddRange(values.Select(item => (Number)item).ToArray());
        }

        public Array(IConstruct[] values)
        {
            items.AddRange(values);
        }
		
		public static implicit operator List<IConstruct>(Array array)
		{
			return array.items;
		}
		
		public static implicit operator Array(IConstruct[] constructs)
		{
			return new Array(constructs);
		}

        public override string ToString()
        {
			return "Array";
        }

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return items.GetEnumerator();
		}

		IEnumerator<IConstruct> IEnumerable<IConstruct>.GetEnumerator ()
		{
			return items.GetEnumerator();
		}
    }
}
