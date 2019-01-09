using System.Collections.Generic;

namespace FileHelpers.Core.Descriptors
{
    public interface IRecordDescriptor
    {
        IDictionary<string, IFieldInfoTypeDescriptor> Fields { get; }

        void Add(string fieldName, IFieldInfoTypeDescriptor fieldDescriptor);
    }
}
