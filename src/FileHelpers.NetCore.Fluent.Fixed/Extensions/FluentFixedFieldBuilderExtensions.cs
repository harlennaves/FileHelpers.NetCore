using System.Text;

using FileHelpers.Core.Converters;
using FileHelpers.Fluent.Fixed.Descriptors;

namespace FileHelpers.Fluent.Fixed.Extensions
{
    public static class FluentFixedFieldBuilderExtensions
    {
        public static string RecordToString(this IFixedFieldInfoDescriptor descriptor, object record)
        {
            var sb = new StringBuilder(descriptor.Length);

            string field = descriptor.CreateFieldString(record);

            descriptor.AlignField(sb, field);

            return sb.ToString();
        }

        public static object StringToRecord(this IFixedFieldInfoDescriptor recordInfo, string line, ref int offset)
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

            return converterInstance?.StringToField(stringValue);
        }

        private static string CreateFieldString(this IFixedFieldInfoDescriptor fieldBuilder, object fieldValue)
        {
            if (fieldBuilder.Converter == null)
            {
                if (fieldValue == null)
                    return string.Empty;
                return fieldValue.ToString();
            }

            ConverterBase converterInstance =
                ConverterFactory.GetConverter(fieldBuilder.Converter, fieldBuilder.ConverterFormat);

            return converterInstance?.FieldToString(fieldValue) ?? string.Empty;
        }

        private static void AlignField(this IFixedFieldInfoDescriptor fieldBuilder, StringBuilder sb, string field)
        {
            if (field.Length > fieldBuilder.Length)
                field = field.Substring(0, fieldBuilder.Length);

            switch (fieldBuilder.AlignMode)
            {
                case AlignMode.Right:
                    sb.Append(field);
                    sb.Append(fieldBuilder.AlignChar, fieldBuilder.Length - field.Length);
                    break;
                case AlignMode.Left:
                    sb.Append(fieldBuilder.AlignChar, fieldBuilder.Length - field.Length);
                    sb.Append(field);
                    break;
                case AlignMode.Center:
                    int middle = (fieldBuilder.Length - field.Length) / 2;

                    sb.Append(fieldBuilder.AlignChar, middle);
                    sb.Append(field);
                    sb.Append(fieldBuilder.AlignChar, fieldBuilder.Length - field.Length - middle);
                    break;
            }
        }

        private static string StringTrim(this IFixedFieldInfoDescriptor recordInfo, string value)
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
