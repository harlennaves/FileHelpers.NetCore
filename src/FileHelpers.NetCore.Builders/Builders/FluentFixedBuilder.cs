using System;
using System.Collections.Generic;

using FileHelpers.NetCore.Builders.Core;

namespace FileHelpers.NetCore.Fluent.Builders
{
    public class FluentFixedBuilder : IFluentFixedBuilder
    {
        public static IFluentFixedBuilder Build(dynamic configuration)
        {
            var builder = new FluentFixedBuilder();

            if (configuration.FixedMode != null)
                builder.FixedMode = (FixedMode)configuration.FixedMode;

            if (configuration.IgnoreEmptyLines != null)
                builder.IgnoreEmptyLines = configuration.IgnoreEmptyLines;

            foreach (dynamic field in configuration.Fields)
            {
                if (field.IsArray == null || !(bool)field.IsArray)
                    AddField(builder, field);
                else if (field.IsArray != null && (bool)field.IsArray)
                    AddArray(builder, field);
            }
            
            return builder;
        }

        private static void AddField(IFluentFixedBuilder builder, dynamic field)
        {
            IFluentFixedFieldBuilder fieldBuilder = builder.Add(field.Name.Value);

            if (field.Length != null)
                fieldBuilder.SetLength((int)field.Length.Value);

            if (field.Align != null)
                fieldBuilder.SetAlign((AlignMode)field.Align.Value);

            if (field.AlignChar != null && !string.IsNullOrWhiteSpace(field.AlignChar.Value))
                fieldBuilder.SetAlignChar(field.AlignChar.Value[0]);

            if (field.Converter != null)
                fieldBuilder.SetConverter(Type.GetType(field.Converter.Value));

            if (field.ConverterFormat != null)
                fieldBuilder.SetConverterFormat(field.ConverterFormat.Value);

            if (field.NullValue != null)
                fieldBuilder.SetNullValue(field.NullValue.Value);

            if (field.TrimMode != null)
                fieldBuilder.SetTrimMode((TrimMode)field.TrimMode.Value);

        }

        private static void AddArray(IFluentFixedBuilder builder, dynamic field)
        {
            IFluentFixedArrayFieldBuilder arrayBuilder = builder.AddArray(field.Name.Value);

            if (field.ArrayLength != null)
                arrayBuilder.SetArrayLength((int)field.ArrayLength.Value);

            if (field.ArrayItemLength != null)
                arrayBuilder.SetArrayItemLength((int)field.ArrayItemLength);

            if (field.Align != null)
                arrayBuilder.SetAlign((bool)field.Align);

            if (field.AlignChar != null && !string.IsNullOrWhiteSpace(field.AlignChar.Value))
                arrayBuilder.SetResidualAlignChar(field.AlignChar.Value[0]);

            if (field.Fields == null)
                return;

            foreach (dynamic arrayField in field.Fields)
            {
                if (arrayField.IsArray == null || !(bool)arrayField.IsArray)
                    AddField(arrayBuilder, arrayField);
                else if (arrayField.IsArray != null && (bool)arrayField.IsArray)
                    AddArray(arrayBuilder, arrayField);
            }
        }

        public FluentFixedBuilder()
        {
            Fields = new Dictionary<string, IFluentFixedRecordInfoBase>();
            FixedMode = FixedMode.ExactLength;
        }

        public IDictionary<string, IFluentFixedRecordInfoBase> Fields { get; }

        public FixedMode FixedMode { get; set; }
        public bool IgnoreEmptyLines { get; set; }

        public IFluentFixedFieldBuilder Add(string fieldName)
        {
            if (Fields.TryGetValue(fieldName, out IFluentFixedRecordInfoBase fieldBuilder))
                return (IFluentFixedFieldBuilder)fieldBuilder;

            fieldBuilder = new FluentFixedFieldBuilder();
            Fields.Add(fieldName, fieldBuilder);
            return (IFluentFixedFieldBuilder)fieldBuilder;
        }

        public IFluentFixedArrayFieldBuilder AddArray(string fieldName)
        {
            if (Fields.TryGetValue(fieldName, out IFluentFixedRecordInfoBase fieldBuilder))
                return (IFluentFixedArrayFieldBuilder)fieldBuilder;

            fieldBuilder = new FluentFixedArrayFieldBuilder();
            Fields.Add(fieldName, fieldBuilder);
            return (IFluentFixedArrayFieldBuilder)fieldBuilder;
        }
    }
}
