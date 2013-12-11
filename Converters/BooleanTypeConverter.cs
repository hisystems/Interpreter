using System;
using System.ComponentModel;
using System.Globalization;

namespace HiSystems.Interpreter.Converters
{
    public class BooleanTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(bool);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Boolean) || destinationType == typeof(Literal);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var boolean = value as Boolean;
            return boolean == null ? default(bool) : (bool)boolean;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var boolean = (bool)value;
            return (Boolean)boolean;
        }
    }
}