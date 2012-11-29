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

