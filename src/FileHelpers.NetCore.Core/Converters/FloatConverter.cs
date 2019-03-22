using System;
using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class FloatConverter : DecimalNumberBaseConverter
    {
        public FloatConverter() : base($"N{CultureInfo.InvariantCulture.NumberFormat.NumberDecimalDigits}")
        {

        }

        public FloatConverter(string format) :
            base(format)
        {

        }

        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new ConvertException(from, typeof(float));

            float to = 0.0F;

            float.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out to);

            if (
                !float.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out float res))
                throw new ConvertException(from, typeof(float));
            return to / Math.Pow(10, DecimalDigits);
        }
    }
}
