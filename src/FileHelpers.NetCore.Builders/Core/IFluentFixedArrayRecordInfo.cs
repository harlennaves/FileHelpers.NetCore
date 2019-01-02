namespace FileHelpers.NetCore.Builders.Core
{
    public interface IFluentFixedArrayRecordInfo : IFluentFixedRecordInfoBase
    {
        int ArrayLength { get; }

        int ArrayItemLength { get; }

        bool Align { get; }

        char AlignChar { get; }
    }
}
