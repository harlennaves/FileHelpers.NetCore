using FileHelpers.Core.Converters;
using FileHelpers.Core.Descriptors;
using System.Text;

namespace FileHelpers.Fluent.Extensions
{
    public static class FieldInfoDescriptorExtensions
    {
        public static string StringTrim(this IFieldInfoDescriptor recordInfo, string value)
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

        public static object StringToField(this IFieldInfoDescriptor fieldDescriptor, string value)
        {
            if (value == null)
                return null;

            value = fieldDescriptor.StringTrim(value);

            if (string.Empty.Equals(value) && fieldDescriptor.Converter == null)
                return value;

            if (fieldDescriptor.Converter == null && fieldDescriptor.Type == null)
                return value;

            ConverterBase converterInstance =
                fieldDescriptor.Converter == null
                ? ConverterFactory.GetDefaultConverter(fieldDescriptor.Type)
                : ConverterFactory.GetConverter(fieldDescriptor.Converter, fieldDescriptor.ConverterFormat);

            return converterInstance == null
                ? value
                : converterInstance.StringToField(value);
        }
    }
}
