using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class DoubleConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new ConvertException(from, typeof(double));

            if (
                !double.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out double res))
                throw new ConvertException(from, typeof(double));
            return res;
        }
    }
}
