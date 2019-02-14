using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileHelpers.Core.Descriptors;
using FileHelpers.Core.Engines;
using FileHelpers.Fluent.Events;
using FileHelpers.Fluent.Exceptions;
using FileHelpers.Fluent.Fixed.Descriptors;
using FileHelpers.Fluent.Fixed.Extensions;
using FileHelpers.Fluent.Fixed.Json;

namespace FileHelpers.Fluent.Fixed
{
    public class FluentFixedEngine : FluentEngineBase
    {
        public static FluentFixedEngine Build(string json) =>
            JsonFixedRecordDescriptorBuilder.Build(json);
        

        public FluentFixedEngine(IRecordDescriptor descriptor) : base(descriptor)
        {
        }

        public FluentFixedEngine(IRecordDescriptor descriptor, Encoding encoding) : base(descriptor, encoding)
        {
        }

        protected override void CheckFieldDescriptor(string fieldName, IFieldInfoTypeDescriptor fieldDescriptor)
        {
            if (fieldDescriptor.IsArray)
            {
                CheckFieldArrayDescriptor(fieldName, fieldDescriptor as IArrayFieldInfoDescriptor);
                return;
            }
            CheckFieldDescriptor(fieldName, fieldDescriptor as IFixedFieldInfoDescriptor);
        }

        private void CheckFieldArrayDescriptor(string fieldName, IArrayFieldInfoDescriptor recordInfo)
        {
            if (recordInfo.ArrayLength <= 0)
                throw new BadFluentConfigurationException($"The property {fieldName} must be the {nameof(recordInfo.ArrayLength)} length greater than 0");

            if (recordInfo.ArrayItemLength <= 0)
                throw new BadFluentConfigurationException($"The property {fieldName} must be the {nameof(recordInfo.ArrayItemLength)} length greater than 0");

            if (recordInfo.ArrayItemLength > recordInfo.ArrayLength)
                throw new BadFluentConfigurationException($"The {nameof(recordInfo.ArrayLength)} of property {fieldName} must be greater than {nameof(recordInfo.ArrayItemLength)}");

            if ((recordInfo.ArrayLength % recordInfo.ArrayItemLength) != 0)
                throw new BadFluentConfigurationException($"The remainder of {nameof(recordInfo.ArrayLength)} division by {nameof(recordInfo.ArrayItemLength)} can not be different than 0");

            var arrayRecordInfo = recordInfo as IRecordDescriptor;

            if (arrayRecordInfo == null)
                throw new BadFluentConfigurationException($"The property {fieldName} is not an array builder");

            CheckDescriptor(arrayRecordInfo, true);
        }

        private void CheckFieldDescriptor(string fieldName, IFixedFieldInfoDescriptor fieldDescriptor)
        {
            if (fieldDescriptor == null)
                throw new ArgumentNullException(nameof(fieldDescriptor));
            if (fieldDescriptor.Length <= 0)
                throw new BadFluentConfigurationException($"The property {fieldName} must be a length gearter than 0");
        }

        public override string WriteString(IEnumerable<ExpandoObject> records)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                WriteStream(writer, records);
                return sb.ToString();
            }
        }

        public override void WriteStream(TextWriter writer, IEnumerable<ExpandoObject> records) =>
            WriteStreamAsync(writer, records).GetAwaiter().GetResult();

        public override async Task WriteStreamAsync(TextWriter writer, IEnumerable<ExpandoObject> records)
        {
            writer.NewLine = Environment.NewLine;
            var lineNumber = 1;
            foreach (ExpandoObject expandoObject in records)
            {
                BeforeFluentWriteEventArgs beforeWriteArgs = OnBeforeWriteRecord(expandoObject, lineNumber);

                var record = (beforeWriteArgs.LineChanged ? beforeWriteArgs.Record : expandoObject) as IDictionary<string, object>;

                if (record == null || beforeWriteArgs.SkipRecord)
                {
                    lineNumber++;
                    continue;
                }
                var sb = new StringBuilder();
                foreach (KeyValuePair<string, object> keyValuePair in record)
                {
                    if (!Descriptor.Fields.TryGetValue(keyValuePair.Key, out IFieldInfoTypeDescriptor fieldDescriptor))
                        throw new Exception($"The field {keyValuePair.Key} is not configured");

                    if (fieldDescriptor.IsArray)
                    {
                        sb.Append(((IArrayFieldInfoDescriptor)fieldDescriptor).ArrayToString(
                            (IEnumerable<dynamic>)keyValuePair.Value));
                        continue;
                    }

                    sb.Append(((IFixedFieldInfoDescriptor)fieldDescriptor).RecordToString(keyValuePair.Value));
                }

                AfterFluentWriteEventArgs afterWriteArgs = OnAfterWriteRecord(expandoObject, lineNumber, sb.ToString());

                await writer.WriteLineAsync(afterWriteArgs.Line);
                await writer.FlushAsync();
                lineNumber++;
            }
        }

        public override void WriteFile(string fileName, IEnumerable<ExpandoObject> records) =>
            WriteFileAsync(fileName, records).GetAwaiter().GetResult();

        public override async Task WriteFileAsync(string fileName, IEnumerable<ExpandoObject> records)
        {
            using (var writer = new StreamWriter(fileName))
            {
                await WriteStreamAsync(writer, records);
            }
        }

        public override ExpandoObject[] ReadFile(string fileName) =>
            ReadFileAsync(fileName).GetAwaiter().GetResult();

        public override async Task<ExpandoObject[]> ReadFileAsync(string fileName)
        {
            using (var reader = new StreamReader(fileName))
                return await ReadStreamAsync(reader);
        }

        public override ExpandoObject[] ReadString(string source)
        {
            if (source == null)
                source = string.Empty;
            using (var stream = new MemoryStream(Encoding.GetBytes(source)))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return ReadStream(streamReader);
                }
            }
        }

        public override ExpandoObject[] ReadStream(StreamReader reader) =>
            ReadStreamAsync(reader).GetAwaiter().GetResult();


        protected override async Task<ExpandoObject> ReadLineAsync(string currentLine, IRecordDescriptor descriptor)
        {
            return await Task.Run((() =>
            {
                var item = new ExpandoObject();

                var offset = 0;
                foreach (KeyValuePair<string, IFieldInfoTypeDescriptor> fieldInfoTypeDescriptor in descriptor.Fields)
                {
                    if (fieldInfoTypeDescriptor.Value.IsArray)
                    {
                        item.TryAdd(fieldInfoTypeDescriptor.Key,
                            ((IArrayFieldInfoDescriptor)fieldInfoTypeDescriptor.Value).StringToArray(currentLine,
                                ref offset));
                        continue;
                    }

                    item.TryAdd(fieldInfoTypeDescriptor.Key,
                        ((IFixedFieldInfoDescriptor)fieldInfoTypeDescriptor.Value).StringToRecord(currentLine, ref offset));
                }

                return item;
            }));
        }

        public override async Task<ExpandoObject[]> ReadStreamAsync(StreamReader reader)
        {
            IList<ExpandoObject> items = new List<ExpandoObject>();

            string currentLine = await reader.ReadLineAsync();
            int currentLineNumber = 1;
            while (currentLine != null)
            {
                if (!string.IsNullOrWhiteSpace(currentLine))
                {
                    BeforeFluentReadEventArgs beforeReadArgs = OnBeforeReadRecord(currentLine, currentLineNumber);
                    if (!beforeReadArgs.SkipRecord)
                    {
                        if (beforeReadArgs.LineChanged)
                            currentLine = beforeReadArgs.Line;

                        ExpandoObject item = await ReadLineAsync(currentLine, Descriptor);
                        
                        AfterFluentReadEventArgs afterReadArgs = OnAfterReadRecord(currentLine, currentLineNumber, item);

                        items.Add(afterReadArgs.Record);
                    }
                }
                currentLineNumber++;
                currentLine = await reader.ReadLineAsync();
            }

            return items.ToArray();
        }

        public override string Serialize() =>
            JsonFixedRecordDescriptorBuilder.Serialize((FixedRecordDescriptor)Descriptor);
    }
}
