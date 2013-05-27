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

namespace HiSystems.Interpreter
{
    public class Text : Literal
    {
        private string value;

        public Text(string value)
        {
            this.value = value;
        }
        
        public static implicit operator string(Text text)
        {
            return text.value;
        }

        public static implicit operator Text(string text)
        {
            return new Text(text);
        }

        public static Boolean operator==(Text value1, Text value2)
        {
            return AreEqual(value1, value2);
        }
        
        public static Boolean operator!=(Text value1, Text value2)
        {
            return !AreEqual(value1, value2);
        }

        public static Text operator+(Text value1, Text value2)
        {
            return new Text(value1.value + value2.value);
        }

		/// <summary>
		/// Returns the length of the text.
		/// </summary>
        public int Length
        {
            get
            {
                return this.value.Length;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Text))
                return false;
            else 
                return AreEqual(this, (Text)obj);
        }
        
        private static Boolean AreEqual(Text value1, Text value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return new Boolean(false);
            else
                return new Boolean(value1.value.Equals(value2.value, StringComparison.InvariantCulture));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        } 
        
        public override string ToString()
        {
            return value.ToString();
        }
    }
}

