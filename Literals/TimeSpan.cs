using System.ComponentModel;
using HiSystems.Interpreter.Converters;

namespace HiSystems.Interpreter {
	/// <summary>
	/// Represents an immutable date / time value.
	/// </summary>
	[TypeConverter(typeof(DateTimeTypeConverter))]
	public class TimeSpan : Literal
	{
		private System.TimeSpan value;

		public TimeSpan(System.TimeSpan value)
		{
			this.value = value;
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator System.TimeSpan(TimeSpan timeSpan)
		{
			return timeSpan.value;
		}

		public static implicit operator TimeSpan(System.TimeSpan value)
		{
			return new TimeSpan(value);
		}

		public static Boolean operator ==(TimeSpan value1, TimeSpan value2)
		{
			return AreEqual(value1, value2);
		}

		public static Boolean operator !=(TimeSpan value1, TimeSpan value2)
		{
			return !AreEqual(value1, value2);
		}

		public static TimeSpan operator +(TimeSpan date1, TimeSpan date2)
		{
			return new TimeSpan(date1.value + date2.value);
		}

		public static TimeSpan operator -(TimeSpan date1, TimeSpan date2)
		{
			return new TimeSpan(date1.value - date2.value);
		}

		public static Boolean operator >(TimeSpan value1, TimeSpan value2)
		{
			return new Boolean(value1.value > value2.value);
		}

		public static Boolean operator >=(TimeSpan value1, TimeSpan value2)
		{
			return new Boolean(value1.value >= value2.value);
		}

		public static Boolean operator <(TimeSpan value1, TimeSpan value2)
		{
			return new Boolean(value1.value < value2.value);
		}

		public static Boolean operator <=(TimeSpan value1, TimeSpan value2)
		{
			return new Boolean(value1.value <= value2.value);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is TimeSpan))
				return false;
			else
				return AreEqual(this, (TimeSpan)obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		private static Boolean AreEqual(TimeSpan value1, TimeSpan value2)
		{
			if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
				return new Boolean(false);
			else
				return new Boolean(value1.value == value2.value);
		}
	}
}