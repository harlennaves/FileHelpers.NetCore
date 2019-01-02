using System.ComponentModel;
using System.Dynamic;
using System.Text;

using FileHelpers.NetCore.Fluent.Builders;
using FileHelpers.NetCore.Fluent.Events;

namespace FileHelpers.NetCore.Fluent.Engines
{
    /// <summary>
    /// Fluent base for engine events
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FluentEventEngineBase : FluentEngineBase
    {
        public FluentEventEngineBase(IFluentFixedBuilder builder) : base(builder)
        {
        }

        public FluentEventEngineBase(IFluentFixedBuilder builder, Encoding encoding) : base(builder, encoding)
        {
        }

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

            return args;
        }

        protected AfterFluentReadEventArgs OnAfterReadRecord(string line, int lineNumber, ExpandoObject record)
        {
            var args = new AfterFluentReadEventArgs(line, lineNumber)
            {
                Record = record
            };

            AfterReadRecord?.Invoke(this, args);

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
    }
}
