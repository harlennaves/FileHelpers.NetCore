using System;
using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public class UIntegerConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            uint.TryParse(from, NumberStyles.Any, CultureInfo.InvariantCulture, out uint to);
            return (to);
        }
    }
}
