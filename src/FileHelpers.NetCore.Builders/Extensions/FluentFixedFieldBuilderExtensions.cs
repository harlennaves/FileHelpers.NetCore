using System.Text;

using FileHelpers.NetCore.Builders.Core;
using FileHelpers.NetCore.Converters;

namespace FileHelpers.NetCore.Fluent.Extensions
{
    public static class FluentFixedFieldBuilderExtensions
    {
        /// <summary>
        /// Create a string of the object based on a record information supplied
        /// </summary>
        /// <param name="fieldBuilder">Fluent Field Builder used to get field configuration</param>
        /// <param name="record">Object to convert</param>
        /// <returns>String representing the object</returns>
        public static string RecordToString(this IFluentFixedRecordInfo fieldBuilder,  object record)
        {
            var sb = new StringBuilder(fieldBuilder.Length);
            string field = fieldBuilder.CreateFieldString(record);
            
            fieldBuilder.AlignField(sb, field);

            return sb.ToString();
        }

        private static string CreateFieldString(this IFluentFixedRecordInfo fieldBuilder, object fieldValue)
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

        private static void AlignField(this IFluentFixedRecordInfo fieldBuilder, StringBuilder sb, string field)
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
    }
}
