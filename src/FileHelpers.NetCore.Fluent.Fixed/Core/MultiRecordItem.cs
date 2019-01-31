using FileHelpers.Core.Descriptors;

namespace FileHelpers.Fluent.Fixed.Core
{
    public class MultiRecordItem
    {
        public string Name { get; set; }

        public string RegexPattern { get; set; }

        public IRecordDescriptor Descriptor { get; set; }

    }
}
