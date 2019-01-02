using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileHelpers.NetCore.Builders.Core;
using FileHelpers.NetCore.Fluent.Builders;
using FileHelpers.NetCore.Fluent.Events;
using FileHelpers.NetCore.Fluent.Extensions;

using Newtonsoft.Json;

namespace FileHelpers.NetCore.Fluent.Engines
{
    public class FluentFileHelperEngine
        : FluentEventEngineBase
    {
        private static IFluentFixedBuilder CreateBuilder(string json)
        {
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(json);

            return FluentFixedBuilder.Build(jsonObject);
        }

        public FluentFileHelperEngine(string json)
            :base(CreateBuilder(json))
        {

        }

        public FluentFileHelperEngine(IFluentFixedBuilder builder) : base(builder)
        {
        }

        public FluentFileHelperEngine(IFluentFixedBuilder builder, Encoding encoding) : base(builder, encoding)
        {
        }

        public string WriteString(IEnumerable<ExpandoObject> records)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                WriteStream(writer, records);
                return sb.ToString();
            }
        }

        public void WriteStream(TextWriter writer, IEnumerable<ExpandoObject> records) => 
            WriteStreamAsync(writer, records).GetAwaiter().GetResult();

        public async Task WriteStreamAsync(TextWriter writer, IEnumerable<ExpandoObject> records)
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
                    if (!Builder.Fields.TryGetValue(keyValuePair.Key, out IFluentFixedRecordInfoBase fieldBuilder))
                        throw new Exception($"The field {keyValuePair.Key} is not configured");
                    if (fieldBuilder.IsArray)
                    {
                        sb.Append(((IFluentFixedArrayRecordInfo)fieldBuilder).ArrayToString((IEnumerable<dynamic>)keyValuePair.Value));
                        continue;
                    }
                    sb.Append(((IFluentFixedRecordInfo)fieldBuilder).RecordToString(keyValuePair.Value));
                }

                AfterFluentWriteEventArgs afterWriteArgs = OnAfterWriteRecord(expandoObject, lineNumber, sb.ToString());
                await writer.WriteLineAsync(afterWriteArgs.Line);
                lineNumber++;
            }
        }

        public ExpandoObject[] ReadString(string source)
        {
            if (source == null)
                source = string.Empty;

            using (var reader = new StringReader(source))
            {
                return ReadStream(reader);
            }
        }

        public ExpandoObject[] ReadStream(TextReader reader) => 
            ReadStreamAsync(reader).GetAwaiter().GetResult();

        public async Task<ExpandoObject[]> ReadStreamAsync(TextReader reader)
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

                        var item = new ExpandoObject();
                        var offset = 0;
                        foreach (KeyValuePair<string, IFluentFixedRecordInfoBase> recordInfo in Builder.Fields)
                        {
                            if (recordInfo.Value.IsArray)
                            {
                                item.TryAdd(recordInfo.Key,
                                    ((IFluentFixedArrayRecordInfo)recordInfo.Value).StringToArray(currentLine, ref offset));
                                continue;
                            }

                            item.TryAdd(recordInfo.Key,
                                ((IFluentFixedRecordInfo)recordInfo.Value).StringToRecord(currentLine, ref offset));
                        }

                        AfterFluentReadEventArgs afterReadArgs = OnAfterReadRecord(currentLine, currentLineNumber, item);
                        // ReSharper disable once RedundantAssignment
                        offset = 0;
                        items.Add(afterReadArgs.Record);
                    }
                }

                currentLineNumber++;
                currentLine = await reader.ReadLineAsync();
            }

            return items.ToArray();
        }
    }
}
