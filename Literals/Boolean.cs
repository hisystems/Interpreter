/* _________________________________________________

  (c) Hi-Integrity Systems 2012. All rights reserved.
  www.hisystems.com.au - Toby Wicks
  github.com/hisystems/Interpreter
 
  Licensed under the Apache License, Version 2.0 (the "License");
  you may not use this file except in compliance with the License.
  You may obtain a copy of the License at

      http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS,
  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  See the License for the specific language governing permissions and
  limitations under the License.
 ___________________________________________________ */

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using HiSystems.Interpreter.Converters;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents an immutable boolean value.
    /// </summary>
    [TypeConverter(typeof(BooleanTypeConverter))]
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

		public static Boolean operator ^(Boolean value1, Boolean value2)
		{
			return value1.value ^ value2.value;
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
