using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class ULongConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (!ulong.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out ulong to))
                throw new ConvertException(from, typeof(long));
            return (to);
        }
    }
}
