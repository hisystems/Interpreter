using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace HiSystems.Interpreter.Converters
{
    public class NumberTypeConverter : TypeConverter
    {
        private readonly IEnumerable<Type> _supportedTypes = new[] {typeof (int), typeof (double), typeof (decimal), typeof (Literal)};

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return _supportedTypes.Contains(destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(Number);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value == null ? new Number(0) : new Number((decimal)value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var number = value as Number;

            if (destinationType == typeof(int))
            {
                return number == null ? default(int) : (int) (decimal) number;
            }
            if (destinationType == typeof(decimal))
            {
                return number == null ? default(decimal) : (decimal)number;
            }
            return number == null ? default(double) : (double)number;
        }
    }
}