namespace FileHelpers.Core.Descriptors
{
    public interface IArrayFieldInfoDescriptor : IFieldInfoTypeDescriptor, IXmlFieldPropertyNameInfoDescriptor, IRecordDescriptor
    {
        int ArrayLength { get; set; }

        int ArrayItemLength { get; set; }

        bool Align { get; set; }

        char AlignChar { get; set; }
    }
}
