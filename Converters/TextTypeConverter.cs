using System;
using System.ComponentModel;
using System.Globalization;

namespace HiSystems.Interpreter.Converters
{
    public class TextTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || destinationType == typeof(Literal); 
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new Text((value as string) ?? string.Empty);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var text = value as Text;
            return text == null ? string.Empty : (string) text;
        }
    }
}