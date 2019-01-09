using FileHelpers.Fluent.Builders;
using FileHelpers.Fluent.Fixed.Descriptors;

namespace FileHelpers.Fluent.Fixed.Builders
{
    public class FixedFieldInfoBuilder : FieldInfoBuilder, IFixedFieldInfoDescriptor
    {
        public int Length { get; set; }

        public FixedFieldInfoBuilder SetLength(int length)
        {
            Length = length;
            return this;
        }
    }
}
