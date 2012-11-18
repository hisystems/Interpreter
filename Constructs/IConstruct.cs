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
