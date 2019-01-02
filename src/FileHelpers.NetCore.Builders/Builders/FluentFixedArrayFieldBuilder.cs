using System.Collections.Generic;

using FileHelpers.NetCore.Builders.Core;

namespace FileHelpers.NetCore.Fluent.Builders
{
    public class FluentFixedArrayFieldBuilder : IFluentFixedArrayFieldBuilder
    {
        public FluentFixedArrayFieldBuilder()
        {
            Fields = new Dictionary<string, IFluentFixedRecordInfoBase>();
            FixedMode = FixedMode.ExactLength;
            IgnoreEmptyLines = true;
            AlignChar = ' ';
        }

        public bool IsArray => true;

        public IDictionary<string, IFluentFixedRecordInfoBase> Fields { get; }
        public FixedMode FixedMode { get; }
        public bool IgnoreEmptyLines { get; set; }

        public IFluentFixedFieldBuilder Add(string fieldName)
        {
            if (Fields.TryGetValue(fieldName, out IFluentFixedRecordInfoBase fieldBuilder))
                return (IFluentFixedFieldBuilder)fieldBuilder;

            fieldBuilder = new FluentFixedFieldBuilder();
            Fields.Add(fieldName, fieldBuilder);
            return (IFluentFixedFieldBuilder)fieldBuilder;
        }

        public IFluentFixedArrayFieldBuilder AddArray(string fieldName)
        {
            if (Fields.TryGetValue(fieldName, out IFluentFixedRecordInfoBase fieldBuilder))
                return (IFluentFixedArrayFieldBuilder)fieldBuilder;

            fieldBuilder = new FluentFixedArrayFieldBuilder();
            Fields.Add(fieldName, fieldBuilder);
            return (IFluentFixedArrayFieldBuilder)fieldBuilder;
        }

        public int ArrayLength { get; private set; }
        public int ArrayItemLength { get; private set; }
        public bool Align { get; private set; }
        public char AlignChar { get; private set; }

        public IFluentFixedArrayFieldBuilder SetArrayLength(int length)
        {
            ArrayLength = length;
            return this;
        }

        public IFluentFixedArrayFieldBuilder SetArrayItemLength(int length)
        {
            ArrayItemLength = length;
            return this;
        }
        public IFluentFixedArrayFieldBuilder SetAlign(bool align)
        {
            Align = align;
            return this;
        }

        public IFluentFixedArrayFieldBuilder SetResidualAlignChar(char alignChar)
        {
            AlignChar = alignChar;
            return this;
        }
    }
}
