using FileHelpers.Core.Converters;
using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Extensions;

namespace FileHelpers.Fluent.Delimited.Extensions
{
    internal static class DelimitedFieldBuilderDescriptorExtensions
    {
        internal static string RecordToString(this IFieldInfoDescriptor fieldInfoDescriptor, object record) =>
            fieldInfoDescriptor.CreateFieldString(record);

        private static string CreateFieldString(this IFieldInfoDescriptor fieldInfoDescriptor, object fieldValue)
        {
            if (fieldInfoDescriptor.Converter == null)
            {
                if (fieldValue == null)
                    return string.Empty;
                return fieldValue.ToString();
            }

            ConverterBase converterInstance =
                ConverterFactory.GetConverter(fieldInfoDescriptor.Converter, fieldInfoDescriptor.ConverterFormat);

            return converterInstance?.FieldToString(fieldValue) ?? string.Empty;
        }

        internal static object StringToRecord(this IFieldInfoDescriptor fieldInfoDescriptor, string fieldString, char nullChar)
        {
            if (fieldString == null)
                return fieldInfoDescriptor.NullValue ?? null;

            string stringNullRepresentation = new string(nullChar, fieldString.Length);

            if (fieldString == stringNullRepresentation)
                return fieldInfoDescriptor.NullValue ?? null;

            fieldString = fieldInfoDescriptor.StringTrim(fieldString);
            ConverterBase converterInstance;
            if (string.Empty.Equals(fieldString) && fieldInfoDescriptor.Converter == null)
            {
                if (fieldInfoDescriptor.NullValue != null)
                    fieldString = fieldInfoDescriptor.NullValue.ToString();
                if (string.Empty.Equals(fieldString) && fieldInfoDescriptor.Converter == null)
                {
                    if (fieldInfoDescriptor.Type != null)
                    {
                        converterInstance = ConverterFactory.GetDefaultConverter(fieldInfoDescriptor.Type);
                        return converterInstance == null
                            ? fieldString
                            : converterInstance.StringToField(fieldString);
                    }
                    return fieldString;
                }
            }

            if (fieldInfoDescriptor.Converter == null && fieldInfoDescriptor.Type == null)
                return fieldString;

            if (string.IsNullOrWhiteSpace(fieldString) && fieldInfoDescriptor.NullValue != null)
                fieldString = fieldInfoDescriptor.NullValue.ToString();

            converterInstance =
                fieldInfoDescriptor.Converter == null
                ? ConverterFactory.GetDefaultConverter(fieldInfoDescriptor.Type)
                : ConverterFactory.GetConverter(fieldInfoDescriptor.Converter, fieldInfoDescriptor.ConverterFormat);

            return converterInstance == null
                ? fieldString
                : converterInstance.StringToField(fieldString);
        }
    }
}
