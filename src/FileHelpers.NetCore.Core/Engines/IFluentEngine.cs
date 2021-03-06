﻿using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

using FileHelpers.Fluent.Events;

namespace FileHelpers.Core.Engines
{
    public interface IFluentEngine
    {
        string WriteString(IEnumerable<ExpandoObject> records);

        void WriteStream(TextWriter writer, IEnumerable<ExpandoObject> records, bool flush = true);

        Task WriteStreamAsync(TextWriter writer, IEnumerable<ExpandoObject> records, bool flush = true);

        ExpandoObject[] ReadString(string source);

        ExpandoObject[] ReadStream(StreamReader reader);

        Task<ExpandoObject[]> ReadStreamAsync(StreamReader reader);

        ExpandoObject[] ReadBuffer(byte[] buffer);

        Task<ExpandoObject[]> ReadBufferAsync(byte[] buffer);

        /// <summary>
        /// Called in read operations just before the record string is
        /// translated to a record.
        /// </summary>
        event BeforeFluentReadHandler BeforeReadRecord;

        /// <summary>
        /// Called in read operations just after the record was created from a
        /// record string.
        /// </summary>
        event AfterFluentReadHandler AfterReadRecord;

        /// <summary>
        /// Called in write operations just before the record is converted to a
        /// string to write it.
        /// </summary>
        event BeforeFluentWriteHandler BeforeWriteRecord;

        /// <summary>
        /// Called in write operations just after the record was converted to a
        /// string.
        /// </summary>
        event AfterFluentWriteHandler AfterWriteRecord;
    }
}
