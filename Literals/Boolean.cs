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

        public static Boolean operator==(Boolean value1, Boolean value2)
        {
            return AreEqual(value1, value2);
        }

        public static Boolean operator!=(Boolean value1, Boolean value2)
        {
            return !AreEqual(value1, value2);
        }
        
        public static Boolean operator&(Boolean value1, Boolean value2)
        {
            return value1.value & value2.value;
        }

        public static Boolean operator|(Boolean value1, Boolean value2)
        {
            return value1.value | value2.value;
        }

        public static Boolean operator!(Boolean value)
        {
            return new Boolean(!value.value);
        }
        
        public static bool operator true(Boolean value)
        {
            return value.value;
        }
        
        public static bool operator false(Boolean value)
        {
            return !value.value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals (object obj)
        {
            if (obj == null || !(obj is Boolean))
                return false;
            else 
                return AreEqual(this, (Boolean)obj);
        }

        private static Boolean AreEqual(Boolean value1, Boolean value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return new Boolean(false);
            else
                return new Boolean(value1.value == value2.value);
        }
    }
}
