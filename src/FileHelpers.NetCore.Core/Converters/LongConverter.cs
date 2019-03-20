using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class LongConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            long.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out long to);
               
            return (to);
        }
    }
}
