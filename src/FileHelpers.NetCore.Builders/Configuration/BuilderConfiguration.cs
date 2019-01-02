using System.Collections.Generic;

namespace FileHelpers.NetCore.Fluent.Configuration
{
    public class BuilderConfiguration
    {
        public FixedMode FixedMode { get; set; }

        public bool IgnoreEmptyLines { get; set; }

        public IList<FieldConfigurationBase> Fields { get; set; }
    }
}
