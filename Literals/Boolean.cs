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
    /// Represents an immutable boolean value.
    /// </summary>
    public class Boolean : Literal
    {
        private bool value;

        public Boolean(bool value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
		
		public static implicit operator bool(Boolean boolean)
		{
			return boolean.value;
		}
		
		public static implicit operator Boolean(bool value)
		{
			return new Boolean(value);
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null || !(obj is Boolean))
				return false;
			else 
				return ((Boolean)obj).value == this.value;
		}
    }
}
