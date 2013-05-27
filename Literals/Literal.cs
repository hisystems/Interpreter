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
    /// <summary>
    /// Base class for all literals / concrete values, such as a number, boolean, string etc.
    /// </summary>
    public abstract class Literal : IConstruct
    {
        /// <summary>
        /// A literal does not need to be transformed or executed like a function or operation.
        /// So return this construct as is.
        /// </summary>
        Literal IConstruct.Transform()
        {
            return this;
        }
    }
}

