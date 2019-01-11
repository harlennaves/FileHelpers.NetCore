using System;

using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Fixed.Core;
using FileHelpers.Fluent.Fixed.Descriptors;
using FileHelpers.Fluent.Fixed.Extensions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FileHelpers.Fluent.Fixed.Json
{
    internal static class JsonFixedRecordDescriptorBuilder
    {
        public static FluentFixedEngine Build(string json)
        {
            dynamic deserializedBuilderInfo = JObject.Parse(json);
            
            var descriptor = new FixedRecordDescriptor();

            if (deserializedBuilderInfo.FixedMode != null)
                descriptor.FixedMode = (FixedMode)deserializedBuilderInfo.FixedMode.Value;

            if (deserializedBuilderInfo.IgnoreEmptyLines != null)
                descriptor.IgnoreEmptyLines = deserializedBuilderInfo.IgnoreEmptyLines.Value;

            if (deserializedBuilderInfo.Fields != null)
            {
                foreach (dynamic fieldInfo in deserializedBuilderInfo.Fields)
                {
                    if (fieldInfo.Name == null || fieldInfo.Value == null)
                        continue;
                    if (fieldInfo.Value.IsArray != null && (bool)fieldInfo.Value.IsArray.Value)
                    {
                        CreateArray(descriptor, fieldInfo);
                        continue;
                    }
                    CreateField(descriptor, fieldInfo);
                }
            }

            return descriptor.Build();
        }

        private static void CreateField(IRecordDescriptor descriptor, dynamic fieldInfo)
        {
            var fieldBuilder = descriptor.AddField((string)fieldInfo.Name);

            dynamic fieldInfoValue = fieldInfo.Value;

            if (fieldInfoValue.Length != null)
                fieldBuilder.SetLength((int)fieldInfoValue.Length.Value);
            if (fieldInfoValue.Converter != null)
                fieldBuilder.SetConverter(Type.GetType(fieldInfoValue.Converter.Value));
            if (fieldInfoValue.NullValue != null)
                fieldBuilder.SetNullValue(fieldInfoValue.NullValue.Value);
            if (fieldInfoValue.AlignMode != null)
                fieldBuilder.SetAlignMode((AlignMode)fieldInfoValue.AlignMode.Value);
            if (fieldInfoValue.AlignChar != null && !string.IsNullOrWhiteSpace((string)fieldInfoValue.AlignChar.Value))
                fieldBuilder.SetAlignChar(((string)fieldInfoValue.AlignChar.Value)[0]);
            if (fieldInfoValue.ConverterFormat != null)
                fieldBuilder.SetConverterFormat((string)fieldInfoValue.ConverterFormat.Value);
            if (fieldInfoValue.TrimMode != null)
                fieldBuilder.SetTrimMode((TrimMode)fieldInfoValue.TrimMode.Value);
        }

        private static void CreateArray(IRecordDescriptor descriptor, dynamic fieldInfo)
        {
            var arrayBuilder = descriptor.AddArray((string)fieldInfo.Name);

            dynamic fieldInfoValue = fieldInfo.Value;

            if (fieldInfoValue.ArrayLength != null)
                arrayBuilder.SetArrayLength((int)fieldInfoValue.ArrayLength.Value);
            if (fieldInfoValue.ArrayItemLength != null)
                arrayBuilder.SetArrayItemLength((int)fieldInfoValue.ArrayItemLength.Value);
            if (fieldInfoValue.Align != null)
                arrayBuilder.SetAlign((bool)fieldInfoValue.Align.Value);
            if (fieldInfoValue.AlignChar != null && !string.IsNullOrWhiteSpace((string)fieldInfoValue.AlignChar.Value))
                arrayBuilder.SetAlignChar(((string)fieldInfoValue.AlignChar.Value)[0]);

            if (fieldInfoValue.Fields == null)
                return;

            foreach (dynamic field in fieldInfoValue.Fields)
            {
                if (field.Name == null || field.Value == null)
                    continue;
                if (field.Value.IsArray != null && (bool)field.Value.IsArray.Value)
                {
                    CreateArray(arrayBuilder, field);
                    continue;
                }
                CreateField(arrayBuilder, field);
            }
        }

        public static string Serialize(FixedRecordDescriptor descriptor)
        {
            return JsonConvert.SerializeObject(descriptor);
        }
    }
}
