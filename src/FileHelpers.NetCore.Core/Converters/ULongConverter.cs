using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class ULongConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            ulong.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out ulong to);
            return (to);
        }
    }
}
