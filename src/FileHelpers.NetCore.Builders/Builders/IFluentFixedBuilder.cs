using System.Collections.Generic;

using FileHelpers.NetCore.Builders.Core;

namespace FileHelpers.NetCore.Fluent.Builders
{
    public interface IFluentFixedBuilder
    {
        IDictionary<string, IFluentFixedRecordInfoBase> Fields { get; }

        FixedMode FixedMode { get; }

        bool IgnoreEmptyLines { get; }

        IFluentFixedFieldBuilder Add(string fieldName);

        IFluentFixedArrayFieldBuilder AddArray(string fieldName);
    }
}
