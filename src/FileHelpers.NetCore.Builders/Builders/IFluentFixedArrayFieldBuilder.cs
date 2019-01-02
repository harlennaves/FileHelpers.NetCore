using FileHelpers.NetCore.Builders.Core;

namespace FileHelpers.NetCore.Fluent.Builders
{
    public interface IFluentFixedArrayFieldBuilder : IFluentFixedArrayRecordInfo, IFluentFixedBuilder
    {
        IFluentFixedArrayFieldBuilder SetArrayLength(int length);

        IFluentFixedArrayFieldBuilder SetArrayItemLength(int length);

        IFluentFixedArrayFieldBuilder SetAlign(bool align);

        IFluentFixedArrayFieldBuilder SetResidualAlignChar(char alignChar);
    }
}
