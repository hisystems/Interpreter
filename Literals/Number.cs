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
    /// Represents an immutable numeric value.
    /// </summary>
    public class Number : Literal
    {
        private decimal value;

        public Number(decimal value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
		
		public static implicit operator decimal(Number number)
		{
			return number.value;
		}
		
		public static implicit operator Number(decimal value)
		{
			return new Number(value);
		}
		
		public static implicit operator Number(double value)
		{
			return new Number((decimal)value);
		}
		
		public static implicit operator Number(int value)
		{
			return new Number((decimal)value);
		}

		public static Number Parse(string value)
		{
			return new Number(Decimal.Parse(value));
		}

		public override bool Equals (object obj)
		{
			if (obj == null || !(obj is Number))
				return false;
			else 
				return ((Number)obj).value == this.value;
		}
    }
}
