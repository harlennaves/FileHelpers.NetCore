using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Builders;
using FileHelpers.Fluent.Delimited.Builders;
using FileHelpers.Fluent.Delimited.Descriptors;
using FileHelpers.Fluent.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileHelpers.Fluent.Delimited.Extensions
{
    public static class DelimitedRecordDescriptorExtensions
    {
        public static FieldInfoBuilder AddField(this DelimitedRecordDescriptor descriptor, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadFluentConfigurationException($"The {nameof(name)} cannot be null or empty");

            var fieldInfo = new FieldInfoBuilder();

            descriptor.Add(name, fieldInfo);

            return fieldInfo;
        }

        public static FieldInfoBuilder AddField(this DelimitedArrayFieldInfoBuilder descriptor, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadFluentConfigurationException($"The {nameof(name)} cannot be null or empty");

            var fieldInfo = new FieldInfoBuilder();

            descriptor.Add(name, fieldInfo);

            return fieldInfo;
        }

        public static DelimitedArrayFieldInfoBuilder AddArray(this IRecordDescriptor descriptor, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BadFluentConfigurationException($"The {nameof(name)} cannot be null or empty");

            var fieldInfo = new DelimitedArrayFieldInfoBuilder();
            descriptor.Add(name, fieldInfo);
            return fieldInfo;
        }
    }
}
