using System;
using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class ByteConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            if (string.IsNullOrWhiteSpace(from))
                return null;
            byte.TryParse(from.RemoveBlanks(), NumberStyles.Number, CultureInfo.InvariantCulture, out byte res);
            return res;
        }
    }
}
