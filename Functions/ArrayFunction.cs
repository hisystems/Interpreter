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
    /// Returns an array from a list of items.
    /// Usage: Array(item1, item2, ...)
    /// Example: Array(1, 2, 3)
    /// </summary>
    public class ArrayFunction : Function
    {
        public override string Name
        {
            get
            {
                return "Array";
            }
        }

        public override Literal Execute(IConstruct[] arguments)
        {
            base.EnsureArgumentCountIsAtLeast(arguments, 1);
            
            return new Array(arguments.Select(argument => argument.Transform()).ToArray());
        }
    }
}
