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
    internal class PeekableEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> peekingEnumerator;
        private T current = default(T);
        private T peeked;

        public PeekableEnumerator(IEnumerable<T> enumerable)
        {
            this.peekingEnumerator = enumerable.GetEnumerator();
            MoveNextInternal();
        }

        public T Current
        {
            get
            {
                return current;
            }
        }

        /// <summary>
        /// Returns the item ahead of the current item being enumerated.
        /// </summary>
        public T Peek
        {
            get
            {
                return peeked;
            }
        }

        /// <summary>
        /// Indicates whether there is another item ahead of the current item.
        /// </summary>
        public bool CanPeek
        {
            get
            {
                return peeked != null;
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get 
            {
                return current;
            }
        }

        public bool MoveNext()
        {
            this.current = this.peeked;

            // if reached end of the enumeration
            if (Object.Equals(this.current, default(T)))
                return false;
            else
            {
                MoveNextInternal();
                return true;
            }
        }

        private void MoveNextInternal()
        {
            if (this.peekingEnumerator.MoveNext())
                this.peeked = this.peekingEnumerator.Current;
            else
                this.peeked = default(T);
        }

        public void Reset()
        {
            this.peekingEnumerator.Reset();
            
            MoveNextInternal();
        }

        public void Dispose()
        {
            this.peekingEnumerator.Dispose();
        }
    }
}
