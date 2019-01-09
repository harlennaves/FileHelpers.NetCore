using FileHelpers.Core.Descriptors;

using FileHelpers.Fluent.Fixed.Core;

namespace FileHelpers.Fluent.Fixed.Descriptors
{
    public class FixedRecordDescriptor : RecordDescriptor
    {
        FixedMode FixedMode { get; }

        bool IgnoreEmptyLines { get; }

        public FixedRecordDescriptor()
        {
            FixedMode = FixedMode.ExactLength;
            IgnoreEmptyLines = true;
        }

        public FluentFixedEngine Build() => 
            new FluentFixedEngine(this);
    }
}
