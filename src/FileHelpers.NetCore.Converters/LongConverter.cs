using System.Globalization;

namespace FileHelpers.NetCore.Converters
{
    public class LongConverter : ConverterBase
    {
        public LongConverter() { }

        public override object StringToField(string from)
        {
            long to = 0;
            long.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out to);
            return (to);
        }
    }
}