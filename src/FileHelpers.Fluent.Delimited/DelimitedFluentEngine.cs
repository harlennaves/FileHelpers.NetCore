using FileHelpers.Core.Descriptors;
using FileHelpers.Core.Engines;
using FileHelpers.Fluent.Delimited.Descriptors;
using FileHelpers.Fluent.Delimited.Extensions;
using FileHelpers.Fluent.Events;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelpers.Fluent.Delimited
{
    public class DelimitedFluentEngine : FluentEngineBase
    {
        public DelimitedFluentEngine(IRecordDescriptor descriptor) : base(descriptor)
        {
        }

        public DelimitedFluentEngine(IRecordDescriptor descriptor, Encoding encoding) : base(descriptor, encoding)
        {
        }

        public override ExpandoObject[] ReadFile(string fileName) => 
            ReadFileAsync(fileName).GetAwaiter().GetResult();

        public override Task<ExpandoObject[]> ReadFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public override ExpandoObject[] ReadStream(StreamReader reader) =>
            ReadStreamAsync(reader).GetAwaiter().GetResult();

        public override ExpandoObject[] ReadBuffer(byte[] buffer) =>
            ReadBufferAsync(buffer).GetAwaiter().GetResult();

        public override Task<ExpandoObject[]> ReadBufferAsync(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
                using (var streamReader = new StreamReader(stream, Encoding))
                    return ReadStreamAsync(streamReader);
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

        public override ExpandoObject[] ReadString(string source)
        {
            if (source == null)
                source = string.Empty;

            using (var stream = new MemoryStream(Encoding.GetBytes(source)))
                using (var streamReader = new StreamReader(stream))
                    return ReadStream(streamReader);
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }

        public override void WriteFile(string fileName, IEnumerable<ExpandoObject> records) =>
            WriteFileAsync(fileName, records).GetAwaiter().GetResult();

        public override async Task WriteFileAsync(string fileName, IEnumerable<ExpandoObject> records)
        {
            using (var writer = new StreamWriter(fileName))
                await WriteStreamAsync(writer, records);
        }

        public override void WriteStream(TextWriter writer, IEnumerable<ExpandoObject> records) => 
            WriteStreamAsync(writer, records).GetAwaiter().GetResult();

        public override async Task WriteStreamAsync(TextWriter writer, IEnumerable<ExpandoObject> records)
        {
            writer.NewLine = Environment.NewLine;
            var lineNumber = 1;
            string delimiter = ((DelimitedRecordDescriptor)Descriptor).Delimiter;
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
                        sb.Append(delimiter);
                        continue;
                    }
                   sb.Append(
                        ((IFieldInfoDescriptor)fieldDescriptor).RecordToString(keyValuePair.Value)
                       );
                   sb.Append(delimiter);
                }

                AfterFluentWriteEventArgs afterWriteArgs = OnAfterWriteRecord(expandoObject, lineNumber, sb.ToString());

                await writer.WriteLineAsync(afterWriteArgs.Line);
                await writer.FlushAsync();
                lineNumber++;
            }
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

        protected override void CheckFieldDescriptor(string fieldName, IFieldInfoTypeDescriptor fieldDescriptor)
        {
            
        }

        protected override Task<ExpandoObject> ReadLineAsync(string currentLine, IRecordDescriptor descriptor)
        {
            return Task.Run(() =>
            {
                var item = new ExpandoObject();
                string[] fieldsValue = currentLine.Split(((DelimitedRecordDescriptor)Descriptor).Delimiter);
                int index = 0;
                foreach (KeyValuePair<string, IFieldInfoTypeDescriptor> fieldInfoTypeDescriptor in descriptor.Fields)
                {
                    string fieldValue = index >= fieldsValue.Length ? null : fieldsValue[index];
                    item.TryAdd(
                            fieldInfoTypeDescriptor.Key,
                            fieldInfoTypeDescriptor.Value.IsArray
                                ? ((IDelimitedArrayFieldInfoDescriptor)fieldInfoTypeDescriptor.Value)
                                    .StringToArray(fieldValue, Descriptor.NullChar)
                                : ((IFieldInfoDescriptor)fieldInfoTypeDescriptor.Value)
                                    .StringToRecord(fieldValue, Descriptor.NullChar)
                        );

                    index++;
                }

                return item;
            });
        }
    }
}
