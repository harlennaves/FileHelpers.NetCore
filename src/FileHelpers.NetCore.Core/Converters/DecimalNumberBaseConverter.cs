using FileHelpers.Fluent.Exceptions;
using System;

namespace FileHelpers.Core.Converters
{
    public abstract class DecimalNumberBaseConverter : ConverterBase
    {
        public string Format { get; }

        public int IntegerDigits { get; private set; }

        public int DecimalDigits {get; private set; }
        public string DecimalSeparator { get; private set; }

        public bool WithSign { get; private set; }

        protected DecimalNumberBaseConverter(string format)
        {
            Format = format;

            DiscoverSeparator(ref format);
            if (!DealWithShortFormat(format))
                DealWithLongFormat(format);
        }

        public override string FieldToString(object from)
        {
            string signValue =
                WithSign
                ? ((decimal)from) > 0 ? "+" : "-"
                : string.Empty;

            var fieldValue = signValue +
                ((decimal)from).ToString(
                    new string('0', IntegerDigits - (string.IsNullOrWhiteSpace(DecimalSeparator) ? 0 : 1)) + "." + new string('0', DecimalDigits));
            if (string.IsNullOrWhiteSpace(DecimalSeparator))
                fieldValue = fieldValue.Replace(".", string.Empty).Replace(",", string.Empty);

            return base.FieldToString(fieldValue);
        }

        private void DiscoverSeparator(ref string pattern)
        {
            pattern = pattern.ToUpperInvariant();
            if (pattern.StartsWith("+NS") || pattern.StartsWith("-NS") || pattern.StartsWith("NS"))
            {
                if (pattern.Contains(","))
                    DecimalSeparator = ",";
                else
                    DecimalSeparator = ".";

                pattern = pattern.Replace("S", string.Empty);
            }
            else
                DecimalSeparator = string.Empty;
        }

        private bool DealWithShortFormat(string pattern)
        {
            pattern = pattern.ToUpperInvariant();
            if (!pattern.StartsWith("+N") && !pattern.StartsWith("-N") && !pattern.StartsWith("N")
                && !pattern.StartsWith("+NS") && !pattern.StartsWith("-NS") && !pattern.StartsWith("NS"))
                return false;
            WithSign = pattern.StartsWith("+") || pattern.StartsWith("-");
            if (WithSign)
                pattern = pattern.Substring(1);

            int numberIntegerDigits = 0;
            int numberDecimalDigits;
            var separator = string.IsNullOrWhiteSpace(DecimalSeparator) ? "." : DecimalSeparator;
            if (pattern.Contains(separator))
            {
                var patternPieces = pattern.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (patternPieces.Length != 2)
                    throw new BadFluentConfigurationException("Invalid pattern!");

                if (!int.TryParse(patternPieces[1], out numberDecimalDigits))
                    throw new BadFluentConfigurationException("The numeric format is invalid!");

                if (!int.TryParse(patternPieces[0].Substring(1), out numberIntegerDigits))
                    throw new BadFluentConfigurationException("The numeric format is invalid!");
            }
            else if (!int.TryParse(pattern.Substring(1), out numberDecimalDigits))
                throw new BadFluentConfigurationException("The numeric format is invalid!");


            DecimalDigits = numberDecimalDigits;
            IntegerDigits = numberIntegerDigits;
            return true;
        }

        private void DealWithLongFormat(string pattern)
        {
            var separator = string.IsNullOrWhiteSpace(DecimalSeparator) ? "." : DecimalSeparator;
            pattern = pattern.ToUpperInvariant();
            WithSign = pattern.StartsWith("+") || pattern.StartsWith("-");
            var patternPieces = pattern.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (patternPieces.Length != 2)
                throw new BadFluentConfigurationException("The numeric format is invalid!");

            DecimalDigits = patternPieces[1].Length;
            IntegerDigits = patternPieces[0].Length - (WithSign ? 1 : 0);
        }
    }
}
