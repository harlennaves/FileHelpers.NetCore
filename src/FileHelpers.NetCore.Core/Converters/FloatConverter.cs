using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class FloatConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new ConvertException(from, typeof(float));

            if (
                !float.TryParse(from.Trim(),
                    NumberStyles.Number | NumberStyles.AllowExponent,
                    CultureInfo.InvariantCulture,
                    out float res))
                throw new ConvertException(from, typeof(float));
            return res;
        }
    }
}
