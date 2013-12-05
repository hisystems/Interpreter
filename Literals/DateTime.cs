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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using HiSystems.Interpreter.Converters;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// Represents an immutable date / time value.
    /// </summary>
    [TypeConverter(typeof(DateTimeTypeConverter))]
    public class DateTime : Literal
    {
        private System.DateTime value;

        public DateTime(System.DateTime value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
        
        public string ToString(string format)
        {
            return value.ToString(format);
        }

        public static implicit operator System.DateTime(DateTime date)
        {
            return date.value;
        }
        
        public static implicit operator DateTime(System.DateTime value)
        {
            return new DateTime(value);
        }

        public static Boolean operator==(DateTime value1, DateTime value2)
        {
            return AreEqual(value1, value2);
        }
        
        public static Boolean operator!=(DateTime value1, DateTime value2)
        {
            return !AreEqual(value1, value2);
        }

        public static DateTime operator+(DateTime date, Number days)
        {
            return new DateTime(date.value.AddDays((double)days));
        }
        
        public static DateTime operator-(DateTime date, Number days)
        {
            return new DateTime(date.value.AddDays(-(double)days));
        }
        
        public static Number operator-(DateTime date1, DateTime date2)
        {
            return new Number(Convert.ToDecimal((date1.value - date2.value).TotalDays));
        }

        public static Boolean operator>(DateTime value1, DateTime value2)
        {
            return new Boolean(value1.value > value2.value);
        }
        
        public static Boolean operator>=(DateTime value1, DateTime value2)
        {
            return new Boolean(value1.value >= value2.value);
        }

        public static Boolean operator<(DateTime value1, DateTime value2)
        {
            return new Boolean(value1.value < value2.value);
        }
        
        public static Boolean operator<=(DateTime value1, DateTime value2)
        {
            return new Boolean(value1.value <= value2.value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DateTime))
                return false;
            else 
                return AreEqual(this, (DateTime)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        private static Boolean AreEqual(DateTime value1, DateTime value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return new Boolean(false);
            else
                return new Boolean(value1.value == value2.value);
        }
    }
}
