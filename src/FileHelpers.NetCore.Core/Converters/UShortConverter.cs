using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class UShortConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            ushort.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out ushort to);
            return (to);
        }
    }
}
