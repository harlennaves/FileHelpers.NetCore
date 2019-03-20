using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class IntegerConverter : ConverterBase
    {
        public IntegerConverter() { }

        public override object StringToField(string from)
        {
            int.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out int to);
            
            return (to);
        }
    }
}
