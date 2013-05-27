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
    /// Interface for all constructs that return a value of some kind:
    ///  function, literal, operation, variable, array.
    /// </summary>
    public interface IConstruct
    {
        /// <summary>
        /// Transforms/executes and returns the value from this construct:
        ///   For Functions: executes the function and returns the result as a literal.
        ///   For Operations: returns the result of the mathematical or logical operation and returns a literal (Number, Boolean for example).
        ///   For Variables: returns the value from the variable's associated construct - i.e. calls Variable.Construct.Transform().
        ///   For Literals: returns the literal value i.e. itself.
        ///   For Arrays: returns the array value, i.e itself.
        /// </summary>
        Literal Transform();
    }
}
