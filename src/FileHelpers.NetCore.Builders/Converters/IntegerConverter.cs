using System.Globalization;

namespace FileHelpers.NetCore.Converters
{
    public class IntegerConverter : ConverterBase
    {
        public IntegerConverter() { }

        public override object StringToField(string from)
        {
            int to = 0;
            int.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out to);
            return (to);
        }
    }
}