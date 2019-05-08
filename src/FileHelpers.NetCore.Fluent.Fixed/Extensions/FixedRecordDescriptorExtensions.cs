using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Exceptions;
using FileHelpers.Fluent.Fixed.Builders;

namespace FileHelpers.Fluent.Fixed.Extensions
{
    public static class FixedRecordDescriptorExtensions
    {
        public static FixedFieldInfoBuilder AddField(this IRecordDescriptor recordInfo, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new BadFluentConfigurationException($"The {nameof(fieldName)} cannot be null or empty");

            var fieldInfo = new FixedFieldInfoBuilder();
            recordInfo.Add(fieldName, fieldInfo);
            
            return fieldInfo;
        }

        public static FixedArrayFieldInfoBuilder AddArray(this IRecordDescriptor recordInfo, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new BadFluentConfigurationException($"The {nameof(fieldName)} cannot be null or empty");

            var fieldInfo = new FixedArrayFieldInfoBuilder();
            recordInfo.Add(fieldName, fieldInfo);
            return fieldInfo;
        }

        public static FixedArrayFieldInfoBuilder AddSubArray(this IRecordDescriptor recordInfo, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new BadFluentConfigurationException($"The {nameof(fieldName)} cannot be null or empty");

            var fieldInfo = new FixedArrayFieldInfoBuilder();
            recordInfo.Add(fieldName, fieldInfo);
            return fieldInfo;
        }

        public static FixedArrayFieldInfoBuilder AddArray(this IFieldInfoTypeDescriptor fieldInfo, string fieldName)
        {
            if (!fieldInfo.IsArray)
                throw new BadFluentConfigurationException($"The parent field must be an array field.");

            var arrayFieldInfoBuilder = fieldInfo as FixedArrayFieldInfoBuilder;
            if (arrayFieldInfoBuilder == null)
                throw new BadFluentConfigurationException("The parent field must be an FixedArrayFieldInfoBuilder instance");

            var subFieldInfo = new FixedArrayFieldInfoBuilder();
            arrayFieldInfoBuilder.Add(fieldName, subFieldInfo);
            
            return subFieldInfo;
        }
    }
}
