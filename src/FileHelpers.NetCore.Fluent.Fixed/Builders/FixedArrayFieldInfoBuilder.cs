using System.Collections.Generic;

using FileHelpers.Core.Descriptors;

namespace FileHelpers.Fluent.Fixed.Builders
{
    public class FixedArrayFieldInfoBuilder : IArrayFieldInfoDescriptor
    {
        public FixedArrayFieldInfoBuilder()
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

        public FixedArrayFieldInfoBuilder SetArrayLength(int arrayLength)
        {
            ArrayLength = arrayLength;
            return this;
        }

        public FixedArrayFieldInfoBuilder SetArrayItemLength(int arrayItemLength)
        {
            ArrayItemLength = arrayItemLength;
            return this;
        }

        public FixedArrayFieldInfoBuilder SetAlign(bool align)
        {
            Align = align;
            return this;
        }

        public FixedArrayFieldInfoBuilder SetAlignChar(char alignChar)
        {
            AlignChar = alignChar;
            return this;
        }
    }
}
