using System;

using FileHelpers.NetCore.Builders.Core;

namespace FileHelpers.NetCore.Fluent.Builders
{
    public interface IFluentFixedFieldBuilder
    {
        IFluentFixedFieldBuilder SetLength(int length);

        IFluentFixedFieldBuilder SetConverter(Type converterType);

        IFluentFixedFieldBuilder SetNullValue(object nullValue);

        IFluentFixedFieldBuilder SetAlign(AlignMode align);

        IFluentFixedFieldBuilder SetAlignChar(char alignChar);

        IFluentFixedFieldBuilder SetConverterFormat(string format);

        IFluentFixedFieldBuilder SetTrimMode(TrimMode trimMode);

        IFluentFixedFieldBuilder SetArrayLength(int length);

        IFluentFixedArrayFieldBuilder SetArray();
    }
}
  