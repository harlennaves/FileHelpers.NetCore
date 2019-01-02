using System;
using System.Globalization;

namespace FileHelpers.NetCore.Converters
{
    public class DoubleConverter : ConverterBase
    {
        private int _integerPlaces;
        private int _decimalPlaces;
        private bool _withLeftSign;

        public DoubleConverter(int integerPlaces, int decimalPlaces)
        {
            _integerPlaces = integerPlaces;
            _decimalPlaces = decimalPlaces;
            _withLeftSign = false;
        }

        public DoubleConverter(int integerPlaces, int decimalPlaces, bool withLeftSign)
        {
            _integerPlaces = integerPlaces;
            _decimalPlaces = decimalPlaces;
            _withLeftSign = withLeftSign;
        }

        public override object StringToField(string from)
        {
            double to = 0.0;
            int signFactor = 1;
            string parsedFrom;

            if (_withLeftSign)
            {
                signFactor = from.Substring(0, 1).Equals("-") ? -1 : 1;
                parsedFrom = from.Substring(1);
            }
            else
            {
                parsedFrom = from;
            }

            double.TryParse(parsedFrom, NumberStyles.Any, CultureInfo.InvariantCulture, out to);
            return ((to / Math.Pow(10, _decimalPlaces)) * signFactor);
        }

        public override string FieldToString(object from)
        {
            string signValue =
                _withLeftSign
                ? ((double)from) > 0 ? "+" : "-"
                : string.Empty;

            return base.FieldToString(
                signValue +
                ((double)from).ToString(
                    new string('0', _integerPlaces) + "." + new string('0', _decimalPlaces))
                    .Replace(".", "").Replace(",", "")
            );
        }
    }
}
