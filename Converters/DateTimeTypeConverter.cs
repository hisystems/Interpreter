using System;
using System.ComponentModel;
using System.Globalization;

namespace HiSystems.Interpreter.Converters
{
    public class DateTimeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (System.DateTime);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(DateTime) || destinationType == typeof(Literal); ;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var date = default(DateTime);
            if (value is System.DateTime)
            {
                date = (System.DateTime) value;
            }

            return new DateTime(date);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var date = value as DateTime;
            if (date != null)
            {
                return (System.DateTime)date;
            }

            return default(System.DateTime);
        }
    }
}