using FileHelpers.Core.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileHelpers.Fluent.Delimited.Descriptors
{
    public class DelimitedRecordDescriptor : RecordDescriptor
    {
        public string Delimiter { get; }

        public bool IgnoreEmptyLines { get; set; }

        public DelimitedRecordDescriptor(string delimiter)
        {
            Delimiter = delimiter;
            IgnoreEmptyLines = true;
            
        }
    }
}
