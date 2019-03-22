using FileHelpers.Core.Converters;
using FileHelpers.Fluent.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileHelpers.Core.Converters
{
    public abstract class DecimalNumberBaseConverter : ConverterBase
    {
        public string Format { get; }

        public int DecimalDigits {get; }
        public string DecimalSeparator { get;}

        protected DecimalNumberBaseConverter(string format)
        {
            Format = format;

            DecimalDigits = GetDecimalDigits(format);
            DecimalSeparator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
        }

        private int GetDecimalDigits(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return CultureInfo.InvariantCulture.NumberFormat.NumberDecimalDigits;

            format = format.ToUpperInvariant();
            if (format.StartsWith("N"))
            {
                if (!int.TryParse(format.Substring(1), out int numberDecimalDigits))
                    throw new BadFluentConfigurationException("The numeric format is invalid!");

                return numberDecimalDigits;
            }
            var formatPieces = format.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (formatPieces.Length != 2)
                throw new BadFluentConfigurationException("The numeric format is invalid!");

            return formatPieces[1].Length;
        }
    }
}
