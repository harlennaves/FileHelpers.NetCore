using FileHelpers.NetCore.Builders.Core;
using FileHelpers.NetCore.Converters;

namespace FileHelpers.NetCore.Fluent.Extensions
{
    public static class FluentFixedFieldBuilderReadExtensions
    {
        public static object StringToRecord(this IFluentFixedRecordInfo recordInfo, string line, ref int offset)
        {
            if (line.Length < recordInfo.Length + offset)
                return null;

            var stringValue = line.Substring(offset, recordInfo.Length);
            offset += recordInfo.Length;

            if (stringValue == null)
                return null;

            stringValue = recordInfo.StringTrim(stringValue);

            if (string.Empty.Equals(stringValue) && recordInfo.Converter == null)
                return stringValue;

            if (recordInfo.Converter == null)
                return stringValue;

            ConverterBase converterInstance =
                ConverterFactory.GetConverter(recordInfo.Converter, recordInfo.ConverterFormat);

            return converterInstance?.StringToField(stringValue) ?? null;
        }

        private static string StringTrim(this IFluentFixedRecordInfo recordInfo, string value)
        {
            switch (recordInfo.TrimMode)
            {
                case TrimMode.None:
                    return value;
                case TrimMode.Both:
                    return value.Trim();
                case TrimMode.Left:
                    return value.TrimStart();
                case TrimMode.Right:
                    return value.TrimEnd();
            }

            return value;
        }
    }
}
