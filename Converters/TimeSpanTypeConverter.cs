using System;
using System.ComponentModel;
using System.Globalization;

namespace HiSystems.Interpreter.Converters {
	public class TimeSpanTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(System.TimeSpan);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(TimeSpan) || destinationType == typeof(Literal); ;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var date = default(TimeSpan);
			if (value is System.TimeSpan)
			{
				date = (System.TimeSpan)value;
			}

			return new TimeSpan(date);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			var date = value as TimeSpan;
			if (date != null)
			{
				return (System.TimeSpan)date;
			}

			return default(System.TimeSpan);
		}
	}
}