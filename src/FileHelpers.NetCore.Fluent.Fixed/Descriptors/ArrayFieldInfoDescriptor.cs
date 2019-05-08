using System.Collections.Generic;

namespace FileHelpers.Core.Descriptors
{
    public abstract class ArrayFieldInfoDescriptor : IArrayFieldInfoDescriptor
    {
        protected ArrayFieldInfoDescriptor()
        {
            IsArray = true;
            Fields = new Dictionary<string, IFieldInfoTypeDescriptor>();
        }

        public bool IsArray { get; }
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

        public int ArrayLength { get; set; }
        public int ArrayItemLength { get; set; }
        public bool Align { get; set; }
        public char AlignChar { get; set; }
        public char NullChar { get; set; }
    }
}
