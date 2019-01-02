using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using FileHelpers.NetCore.Builders.Core;
using FileHelpers.NetCore.Fluent.Builders;
using FileHelpers.NetCore.Fluent.Exceptions;

namespace FileHelpers.NetCore.Fluent.Engines
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class FluentEngineBase
    {
        public IFluentFixedBuilder Builder { get; }

        public Encoding Encoding { get; }

        protected FluentEngineBase(IFluentFixedBuilder builder)
            : this(builder, Encoding.UTF8)
        {
            
        }

        protected FluentEngineBase(IFluentFixedBuilder builder, Encoding encoding)
        {
            CheckBuilder(builder);
            Builder = builder;
            Encoding = encoding;
        }

        private void CheckBuilder(IFluentFixedBuilder builder, bool isArray = false)
        {
            if (!builder.Fields.Any())
                throw new BadFluentConfigurationException(isArray ? "The array property has no fields" : "The builder has no fields");

            foreach (KeyValuePair<string, IFluentFixedRecordInfoBase> fluentFixedRecordInfoBase in builder.Fields)
            {
                if (string.IsNullOrWhiteSpace(fluentFixedRecordInfoBase.Key))
                    throw new BadFluentConfigurationException();

                CheckFieldBuilder(fluentFixedRecordInfoBase.Key, fluentFixedRecordInfoBase.Value);
            }
        }

        private void CheckFieldBuilder(string fieldName, IFluentFixedRecordInfoBase recordInfo)
        {
            if (recordInfo is IFluentFixedRecordInfo)
                CheckFieldBuilder(fieldName, (IFluentFixedRecordInfo)recordInfo);
            else if (recordInfo is IFluentFixedArrayRecordInfo)
                CheckFieldArrayBuilder(fieldName, (IFluentFixedArrayRecordInfo)recordInfo);
            else
                throw new BadFluentConfigurationException($"The property {fieldName} must be (IFluentFixedRecordInfo or IFluentFixedArrayRecordInfo)");
        }

        private void CheckFieldBuilder(string fieldName, IFluentFixedRecordInfo recordInfo)
        {
            if (recordInfo == null)
                throw new ArgumentNullException(nameof(recordInfo));
            if (recordInfo.Length <= 0)
                throw new BadFluentConfigurationException($"The property {fieldName} must be a length gearter than 0");
        }

        private void CheckFieldArrayBuilder(string fieldName, IFluentFixedArrayRecordInfo recordInfo)
        {
            if (recordInfo.ArrayLength <= 0)
                throw new BadFluentConfigurationException($"The property {fieldName} must be the {nameof(recordInfo.ArrayLength)} length greater than 0");

            if (recordInfo.ArrayItemLength <= 0)
                throw new BadFluentConfigurationException($"The property {fieldName} must be the {nameof(recordInfo.ArrayItemLength)} length greater than 0");

            if (recordInfo.ArrayItemLength > recordInfo.ArrayLength)
                throw new BadFluentConfigurationException($"The {nameof(recordInfo.ArrayLength)} of property {fieldName} must be greater than {nameof(recordInfo.ArrayItemLength)}");

            if ((recordInfo.ArrayLength % recordInfo.ArrayItemLength) != 0)
                throw new BadFluentConfigurationException($"The remainder of {nameof(recordInfo.ArrayLength)} division by {nameof(recordInfo.ArrayItemLength)} can not be different than 0");

            var arrayRecordInfo = recordInfo as IFluentFixedBuilder;

            if (arrayRecordInfo == null)
                throw new BadFluentConfigurationException($"The property {fieldName} is not an array builder");

            CheckBuilder(arrayRecordInfo, true);
        }
    }
}
