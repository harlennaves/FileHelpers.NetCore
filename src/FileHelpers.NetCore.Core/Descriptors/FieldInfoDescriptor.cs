using System;

using FileHelpers.Core.Converters;
using FileHelpers.Fluent;
using FileHelpers.Fluent.Exceptions;

namespace FileHelpers.Core.Descriptors
{
    public abstract class FieldInfoDescriptor : IFieldInfoDescriptor
    {
        private Type converter;

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
    }
}
