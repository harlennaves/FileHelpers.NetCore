using FileHelpers.Core.Descriptors;

using FileHelpers.Fluent.Fixed.Core;

namespace FileHelpers.Fluent.Fixed.Descriptors
{
    public class FixedRecordDescriptor : RecordDescriptor
    {
        public FixedMode FixedMode { get; set; }

        public bool IgnoreEmptyLines { get; set; }

        public FixedRecordDescriptor()
        {
            FixedMode = FixedMode.ExactLength;
            IgnoreEmptyLines = true;
        }

        public FluentFixedEngine Build() => 
            new FluentFixedEngine(this);
    }
}
