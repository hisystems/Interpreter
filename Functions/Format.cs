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
using System.Linq;
using System.Text;

namespace HiSystems.Interpreter
{
    /// <summary>
    /// The format for numeric values utilises the standard or custom numeric string formats.
    /// If format is omitted then the value is converted to the most appropriate string representation.
    /// Usage: Format(value [, format])
    /// Example: Format(1, '0.0')
    /// </summary>
    public class Format : Function
    {
        public override string Name
        {
            get
            {
                return "Format";
            }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIsBetween(arguments, 1, 2);

            var value = base.GetTransformedArgument<Literal>(arguments, argumentIndex: 0);

            string format;

            if (arguments.Length > 1)
                format = base.GetTransformedArgument<Text>(arguments, argumentIndex: 1);
            else
                format = null;

            if (value is Number)
            {
                if (format == null)
                    return (Text)((Number)value).ToString();
                else
                    return (Text)((Number)value).ToString(format);
            }
            else if (value is HiSystems.Interpreter.DateTime)
            {
                if (format == null)
                    return (Text)((HiSystems.Interpreter.DateTime)value).ToString();
                else
                    return (Text)((HiSystems.Interpreter.DateTime)value).ToString(format);
            }
            else 
                throw new NotImplementedException(value.GetType().Name);      
        }
    }
}

