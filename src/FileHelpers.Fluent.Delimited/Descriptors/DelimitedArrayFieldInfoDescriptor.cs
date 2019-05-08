using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers.Core.Descriptors;

namespace FileHelpers.Fluent.Delimited.Descriptors
{
    public class DelimitedArrayFieldInfoDescriptor : IDelimitedArrayFieldInfoDescriptor
    {
        public DelimitedArrayFieldInfoDescriptor()
        {
            ArrayDelimiter = "#";
            ArrayItemBegin = "{";
            ArrayItemEnd = "}";
            NullChar = '\u0000';
            Fields = new Dictionary<string, IFieldInfoTypeDescriptor>();
            IsArray = true;
        }

        public string ArrayDelimiter { get; set; }
        public string ArrayItemBegin { get; set; }
        public string ArrayItemEnd { get; set; }
        public char NullChar { get; set; }

        public IDictionary<string, IFieldInfoTypeDescriptor> Fields { get; }

        public bool IsArray { get; }

        public void Add(string fieldName, IFieldInfoTypeDescriptor fieldDescriptor)
        {
            if (Fields.ContainsKey(fieldName))
            {
                Fields[fieldName] = fieldDescriptor;
                return;
            }
            Fields.Add(fieldName, fieldDescriptor);
        }
    }
}
