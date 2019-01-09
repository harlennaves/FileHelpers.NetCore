using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class ShortConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (!short.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out short to))
                throw new ConvertException(from, typeof(long));
            return (to);
        }
    }
}
