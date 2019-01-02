using System;

namespace FileHelpers.NetCore.Builders.Core
{
    public interface IFluentFixedRecordInfo : IFluentFixedRecordInfoBase
    {
        int Length { get; }

        Type Converter { get; }

        object NullValue { get; }

        AlignMode AlignMode { get; }

        char AlignChar { get; }

        string ConverterFormat { get; }

        TrimMode TrimMode { get; }
    }
}
