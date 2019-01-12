using System;

using FileHelpers.Core.Converters;
using FileHelpers.Fluent;
using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Descriptors
{
    public abstract class FieldInfoDescriptor : IFieldInfoDescriptor
    {
        private Type converter;
        private Type type;

        protected FieldInfoDescriptor()
        {
            IsArray = false;
            AlignChar = ' ';
            AlignMode = AlignMode.Right;
            TrimMode = TrimMode.None;
        }

        public bool IsArray { get; }
        public Type Converter
        {
            get => converter;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Converter));
                if (value.BaseType != typeof(ConverterBase))
                    throw new BadFluentConfigurationException("The converter type must inherit from ConverterBase");

                converter = value;
            }
        }
        public object NullValue { get; set; }
        public AlignMode AlignMode { get; set; }
        public char AlignChar { get; set; }
        public string ConverterFormat { get; set; }
        public TrimMode TrimMode { get; set; }
        public Type Type
        {
            get => type;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Type));

                if (value != typeof(bool)
                    && value != typeof(byte)
                    && value != typeof(DateTime)
                    && value != typeof(decimal)
                    && value != typeof(double)
                    && value != typeof(float)
                    && value != typeof(int)
                    && value != typeof(long)
                    && value != typeof(short)
                    && value != typeof(uint)
                    && value != typeof(ulong)
                    && value != typeof(ushort)
                    && value != typeof(sbyte)
                )
                    throw new BadFluentConfigurationException("The type type must be one of bult-in types or an System.DateTime");

                type = value;
            }
        }
    }
}
