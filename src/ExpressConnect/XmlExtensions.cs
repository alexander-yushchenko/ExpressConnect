using System;
using System.Globalization;
using System.Xml.Linq;

namespace AY.TNT.ExpressConnect
{
    public static class XmlExtensions
    {
        public static string GetTextValue(this XContainer element)
        {
            var content = element as XElement;
            return content != null ? content.Value : null;
        }

        public static string GetTextValue(this XAttribute attribute)
        {
            return attribute != null ? attribute.Value : null;
        }

        public static DateTime? GetNullableDateTimeValue(this XContainer element, string format)
        {
            var value = element.GetTextValue();
            if (value == null) return null;
            try
            {
                return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime? GetNullableDateTimeValue(this XAttribute attribute, string format)
        {
            var value = attribute.Value;

            try
            {
                return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static DateTime GetDateTimeValue(this XContainer element, string format)
        {
            var value = element.GetNullableDateTimeValue(format);
            return value ?? DateTime.MinValue;
        }

        public static DateTime GetDateTimeValue(this XAttribute attribute, string format)
        {
            var value = attribute.GetNullableDateTimeValue(format);
            return value ?? DateTime.MinValue;
        }

        public static TimeSpan? GetNullableTimeSpanValue(this XContainer element, string format)
        {
            var value = element.GetTextValue();
            if (value == null) return null;
            try
            {
                return TimeSpan.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static TimeSpan? GetNullableTimeSpanValue(this XAttribute attribute, string format)
        {
            var value = attribute.Value;

            try
            {
                return TimeSpan.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        public static TimeSpan GetTimeSpanValue(this XContainer element, string format)
        {
            var value = element.GetNullableTimeSpanValue(format);
            return value ?? TimeSpan.MinValue;
        }

        public static TimeSpan GetTimeSpanValue(this XAttribute attribute, string format)
        {
            var value = attribute.GetNullableTimeSpanValue(format);
            return value ?? TimeSpan.MinValue;
        }

        public static bool GetBooleanValue(this XContainer element)
        {
            var value = element.GetTextValue();
            if (value == null) return false;
            bool result;
            var parsed = bool.TryParse(value, out result);
            return parsed && result;
        }

        public static int GetInt32Value(this XContainer element)
        {
            var value = element.GetTextValue();
            if (value == null) return 0;
            int result;
            var parsed = int.TryParse(value, out result);
            return parsed ? result : 0;
        }

        public static ushort GetUInt16Value(this XContainer element)
        {
            var value = element.GetTextValue();
            if (value == null) return 0;
            ushort result;
            var parsed = ushort.TryParse(value, out result);
            return (ushort)(parsed ? result : 0);
        }

        public static ushort GetUInt16Value(this XAttribute attribute)
        {
            var value = attribute.Value;
            ushort result;
            var parsed = ushort.TryParse(value, out result);
            return (ushort)(parsed ? result : 0);
        }

        public static double GetDoubleValue(this XContainer element)
        {
            var value = element.GetTextValue();
            if (value == null) return 0.0;
            double result;
            var parsed = double.TryParse(value.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out result);
            return parsed ? result : 0.0;
        }

        public static double GetDoubleValue(this XAttribute attribute)
        {
            var value = attribute.Value;
            double result;
            var parsed = double.TryParse(value.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out result);
            return parsed ? result : 0.0;
        }
    }
}