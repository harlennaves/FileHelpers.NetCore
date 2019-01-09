
using FileHelpers.Core.Descriptors;

namespace FileHelpers.Fluent.Fixed.Descriptors
{
    public interface IFixedFieldInfoDescriptor : IFieldInfoDescriptor
    {
        int Length { get; set; }
    }
}
