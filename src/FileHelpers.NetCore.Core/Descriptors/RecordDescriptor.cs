using System.Collections.Generic;

namespace FileHelpers.Core.Descriptors
{
    public abstract class RecordDescriptor : IRecordDescriptor
    {
        public char NullChar { get; set; }

        protected RecordDescriptor()
        {
            Fields = new Dictionary<string, IFieldInfoTypeDescriptor>();
            NullChar = '\u0000';
        }

        public IDictionary<string, IFieldInfoTypeDescriptor> Fields { get; }

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
