using FileHelpers.Core.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileHelpers.Fluent.Xml.Descriptors
{
    public interface IXmlFieldInfoDescriptor : IFieldInfoDescriptor
    {
        bool IsAttribute { get; set; }

        string PropertyName { get; set; }
    }
}
