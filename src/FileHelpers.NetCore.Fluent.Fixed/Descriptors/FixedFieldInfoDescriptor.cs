using FileHelpers.Core.Descriptors;

namespace FileHelpers.Fluent.Fixed.Descriptors
{
    public class FixedFieldInfoDescriptor : FieldInfoDescriptor, IFixedFieldInfoDescriptor
    {
        public int Length { get; set; }
    }
}
