using System;
using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class DecimalConverter : DecimalNumberBaseConverter
    {
        public DecimalConverter() : base($"N{CultureInfo.InvariantCulture.NumberFormat.NumberDecimalDigits}")
        {

        }

        public DecimalConverter(string format) :
            base(format)
        {

        }

        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new ConvertException(from, typeof(decimal));

            decimal to = 0.0M;

            decimal.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out to);

            if (!string.IsNullOrWhiteSpace(DecimalSeparator))
                return to;

            if (
                !decimal.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out decimal res))
                throw new ConvertException(from, typeof(decimal));
            return to / (decimal)Math.Pow(10, DecimalDigits);
        }
    }
}
