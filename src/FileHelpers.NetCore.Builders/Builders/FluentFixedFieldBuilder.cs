using System;

using FileHelpers.NetCore.Builders.Core;

namespace FileHelpers.NetCore.Fluent.Builders
{
    public class FluentFixedFieldBuilder : IFluentFixedFieldBuilder, IFluentFixedRecordInfo
    {
        public FluentFixedFieldBuilder()
        {
            AlignMode = AlignMode.Right;
            AlignChar = ' ';
        }

        public IFluentFixedFieldBuilder SetLength(int length)
        {
            Length = length;
            return this;
        }

        public IFluentFixedFieldBuilder SetConverter(Type converterType)
        {
            if (converterType.BaseType != typeof(ConverterBase))
                throw new Exception("The converter type must inherit from ConverterBase");

            Converter = converterType;

            return this;
        }

        public IFluentFixedFieldBuilder SetNullValue(object nullValue)
        {
            NullValue = nullValue;
            return this;
        }

        public IFluentFixedFieldBuilder SetAlign(AlignMode align)
        {
            AlignMode = align;

            return this;
        }

        public IFluentFixedFieldBuilder SetAlignChar(char alignChar)
        {
            AlignChar = alignChar;
            return this;
        }

        public IFluentFixedFieldBuilder SetConverterFormat(string format)
        {
            ConverterFormat = format;
            return this;
        }

        public IFluentFixedFieldBuilder SetTrimMode(TrimMode trimMode)
        {
            TrimMode = trimMode;
            return this;
        }

        public IFluentFixedFieldBuilder SetArrayLength(int length)
        {
            ArrayLength = length;
            IsArray = length > 0;
            return this;
        }

        public IFluentFixedArrayFieldBuilder SetArray()
        {
            throw new NotImplementedException();
        }

        public int Length { get; private set; }
        public Type Converter { get; private set; }
        public object NullValue { get; private set; }
        public AlignMode AlignMode { get; private set; }
        public char AlignChar { get; private set; }
        public string ConverterFormat { get; private set; }
        public TrimMode TrimMode { get; private set; }
        public int ArrayLength { get; private set; }
        public bool IsArray { get; private set; }
    }
}
