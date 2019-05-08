using FileHelpers.Fluent.Delimited.Descriptors;

namespace FileHelpers.Fluent.Delimited.Builders
{
    public class DelimitedArrayFieldInfoBuilder : DelimitedArrayFieldInfoDescriptor
    {
        public DelimitedArrayFieldInfoBuilder SetArrayDelimiter(string delimiter)
        {
            ArrayDelimiter = delimiter;
            return this;
        }

        public DelimitedArrayFieldInfoBuilder SetArrayItemBegin(string arrayItemBegin)
        {
            ArrayItemBegin = arrayItemBegin;
            return this;
        }

        public DelimitedArrayFieldInfoBuilder SetArrayItemEnd(string arrayItemEnd)
        {
            ArrayItemEnd = arrayItemEnd;
            return this;
        }
    }
}
