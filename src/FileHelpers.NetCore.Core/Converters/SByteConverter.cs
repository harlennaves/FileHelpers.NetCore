using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class SByteConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                return null;
            sbyte.TryParse(from.RemoveBlanks(), NumberStyles.Number, CultureInfo.InvariantCulture, out sbyte res);
            return res;
        }
    }
}
