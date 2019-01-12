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
            if (!sbyte.TryParse(from.RemoveBlanks(), NumberStyles.Number, CultureInfo.InvariantCulture, out sbyte res))
                throw new ConvertException(from, typeof(sbyte));
            return res;
        }
    }
}
