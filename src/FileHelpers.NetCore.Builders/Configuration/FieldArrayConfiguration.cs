using System.Collections.Generic;

namespace FileHelpers.NetCore.Fluent.Configuration
{
    public class FieldArrayConfiguration : FieldConfigurationBase
    {
        public FieldArrayConfiguration()
        {
            IsArray = true;
            AlignChar = ' ';
            Fields = new List<FieldConfigurationBase>();
        }

        public int ArrayLength { get; set; }

        public int ArrayItemLength { get; set; }

        public bool Align { get; set; }

        public char AlignChar { get; set; }

        public IList<FieldConfigurationBase> Fields { get; set; }
    }
}
