using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Events;
using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Engines
{
    public abstract class FluentEventEngineBase : IFluentEngine
    {
        public Encoding Encoding { get; set; }

        public abstract string WriteString(IEnumerable<ExpandoObject> records);

        public abstract void WriteStream(TextWriter writer, IEnumerable<ExpandoObject> records, bool flush = true);

        public abstract Task WriteStreamAsync(TextWriter writer, IEnumerable<ExpandoObject> records, bool flush = true);

        public abstract void WriteFile(string fileName, IEnumerable<ExpandoObject> records);

        public abstract Task WriteFileAsync(string fileName, IEnumerable<ExpandoObject> records);

        public abstract ExpandoObject[] ReadFile(string fileName);

        public abstract Task<ExpandoObject[]> ReadFileAsync(string fileName);

        public abstract ExpandoObject[] ReadString(string source);

        public abstract ExpandoObject[] ReadStream(StreamReader reader);

        public abstract Task<ExpandoObject[]> ReadStreamAsync(StreamReader reader);

        protected abstract Task<ExpandoObject> ReadLineAsync(string currentLine, IRecordDescriptor descriptor);

        public abstract ExpandoObject[] ReadBuffer(byte[] buffer);

        public abstract Task<ExpandoObject[]> ReadBufferAsync(byte[] buffer);

        /// <summary>
        /// Called in read operations just before the record string is
        /// translated to a record.
        /// </summary>
        public event BeforeFluentReadHandler BeforeReadRecord;

        /// <summary>
        /// Called in read operations just after the record was created from a
        /// record string.
        /// </summary>
        public event AfterFluentReadHandler AfterReadRecord;

        /// <summary>
        /// Called in write operations just before the record is converted to a
        /// string to write it.
        /// </summary>
        public event BeforeFluentWriteHandler BeforeWriteRecord;

        /// <summary>
        /// Called in write operations just after the record was converted to a
        /// string.
        /// </summary>
        public event AfterFluentWriteHandler AfterWriteRecord;

        protected BeforeFluentReadEventArgs OnBeforeReadRecord(string line, int lineNumber)
        {
            var args = new BeforeFluentReadEventArgs(line, lineNumber);

            BeforeReadRecord?.Invoke(this, args);
            args.LineChanged = line != args.Line;
            return args;
        }

        protected AfterFluentReadEventArgs OnAfterReadRecord(string line, int lineNumber, ExpandoObject record)
        {
            var args = new AfterFluentReadEventArgs(line, lineNumber)
            {
                Record = record
            };

            AfterReadRecord?.Invoke(this, args);
            args.LineChanged = line != args.Line;
            return args;
        }

        protected BeforeFluentWriteEventArgs OnBeforeWriteRecord(ExpandoObject record, int lineNumber)
        {
            var args = new BeforeFluentWriteEventArgs(record, lineNumber);

            BeforeWriteRecord?.Invoke(this, args);

            return args;
        }

        protected AfterFluentWriteEventArgs OnAfterWriteRecord(ExpandoObject record, int lineNumber, string line)
        {
            var args = new AfterFluentWriteEventArgs(record, lineNumber, line);

            AfterWriteRecord?.Invoke(this, args);

            return args;
        }

        protected void CheckDescriptor(IRecordDescriptor descriptor, bool isArray = false)
        {
            if (!descriptor.Fields.Any())
                throw new BadFluentConfigurationException(isArray ? "The array property has no fields" : "The builder has no fields");

            foreach (KeyValuePair<string, IFieldInfoTypeDescriptor> fieldInfoDescriptor in descriptor.Fields)
            {
                if (string.IsNullOrWhiteSpace(fieldInfoDescriptor.Key))
                    throw new BadFluentConfigurationException();

                CheckFieldDescriptor(fieldInfoDescriptor.Key, fieldInfoDescriptor.Value);
            }
        }

        protected abstract void CheckFieldDescriptor(string fieldName, IFieldInfoTypeDescriptor fieldDescriptor);
    }
}
