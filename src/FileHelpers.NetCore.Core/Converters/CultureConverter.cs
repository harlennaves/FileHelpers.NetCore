using System;
using System.Globalization;

using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Converters
{
    public abstract class CultureConverter : ConverterBase
    {
        internal const string DefaultDecimalSep = ".";

        protected CultureInfo Culture { get; private set; }

        protected CultureConverter(string decimalSep)
        {
            Culture = CreateCulture(decimalSep);
        }

        private CultureInfo CreateCulture(string decimalSep)
        {
            var ci = new CultureInfo(CultureInfo.CurrentCulture.LCID);

            if (decimalSep == ".")
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                ci.NumberFormat.NumberGroupSeparator = ",";
            }
            else if (decimalSep == ",")
            {
                ci.NumberFormat.NumberDecimalSeparator = ",";
                ci.NumberFormat.NumberGroupSeparator = ".";
            }
            else
                throw new BadFluentConfigurationException("You can only use '.' or ',' as decimal or group separators");

            return ci;
        }

        /// <summary>
        /// Convert the field to a string representation
        /// </summary>
        /// <param name="from">Object to convert</param>
        /// <returns>string representation</returns>
        public sealed override string FieldToString(object from)
        {
            if (from == null)
                return string.Empty;

            return ((IConvertible)from).ToString(Culture);
        }

        /// <summary>
        /// Convert a string to the object type
        /// </summary>
        /// <param name="from">String to convert</param>
        /// <returns>Object converted to</returns>
        public sealed override object StringToField(string from) => 
            ParseString(@from);

        /// <summary>
        /// Convert a string into the return object required
        /// </summary>
        /// <param name="from">Value to convert (string)</param>
        /// <returns>Converted object</returns>
        protected abstract object ParseString(string from);
    }
}
