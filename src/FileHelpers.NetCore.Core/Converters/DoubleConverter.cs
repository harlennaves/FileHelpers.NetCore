using System;
using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class DoubleConverter : DecimalNumberBaseConverter
    {

        public DoubleConverter() : base($"N{CultureInfo.InvariantCulture.NumberFormat.NumberDecimalDigits}")
        {
            
        }

        public DoubleConverter(string format) :
            base(format)
        {
            
        }

        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new ConvertException(from, typeof(double));

            double to = 0.0;

            double.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out to);

            if (
                !double.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out double res))
                throw new ConvertException(from, typeof(double));
            return to / Math.Pow(10, DecimalDigits);
        }
    }
}
