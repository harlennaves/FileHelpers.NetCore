using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class DecimalConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new ConvertException(from, typeof(decimal));

            if (
                !decimal.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out decimal res))
                throw new ConvertException(from, typeof(decimal));
            return res;
        }
    }
}
