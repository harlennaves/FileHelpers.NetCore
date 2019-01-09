using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class IntegerConverter : ConverterBase
    {
        public IntegerConverter() { }

        public override object StringToField(string from)
        {
            if (!int.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out int to))
                throw new ConvertException(from, typeof(int));
            return (to);
        }
    }
}
