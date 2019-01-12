using System;

using FileHelpers.Core.Converters;
using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Fluent.Builders
{
    public abstract class FieldInfoBuilder : IFieldInfoDescriptor
    {
        protected FieldInfoBuilder()
        {
            IsArray = false;
            AlignChar = ' ';
            AlignMode = AlignMode.Right;
            TrimMode = TrimMode.None;
        }

        public bool IsArray { get; }
        public Type Converter { get; set; }
        public object NullValue { get; set; }
        public AlignMode AlignMode { get; set; }
        public char AlignChar { get; set; }
        public string ConverterFormat { get; set; }
        public TrimMode TrimMode { get; set; }
        public Type Type { get; set; }

        public FieldInfoBuilder SetConverter(Type converter)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));
            if (converter.BaseType != typeof(ConverterBase))
                throw new BadFluentConfigurationException("The converter type must inherit from ConverterBase");

            Converter = converter;
            return this;
        }

        public FieldInfoBuilder SetNullValue(object nullValue)
        {
            NullValue = nullValue;
            return this;
        }

        public FieldInfoBuilder SetAlignMode(AlignMode alignMode)
        {
            AlignMode = alignMode;
            return this;
        }

        public FieldInfoBuilder SetAlignChar(char alignChar)
        {
            AlignChar = alignChar;
            return this;
        }

        public FieldInfoBuilder SetConverterFormat(string format)
        {
            ConverterFormat = format;
            return this;
        }

        public FieldInfoBuilder SetTrimMode(TrimMode trimMode)
        {
            TrimMode = trimMode;
            return this;
        }

        public FieldInfoBuilder SetType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type != typeof(bool)
                && type != typeof(byte)
                && type != typeof(DateTime)
                && type != typeof(decimal)
                && type != typeof(double)
                && type != typeof(float)
                && type != typeof(int)
                && type != typeof(long)
                && type != typeof(short)
                && type != typeof(uint)
                && type != typeof(ulong)
                && type != typeof(ushort)
                && type != typeof(sbyte)
                )
                throw new BadFluentConfigurationException("The type type must be one of bult-in types or an System.DateTime");

            Type = type;
            return this;
        }
    }
}
