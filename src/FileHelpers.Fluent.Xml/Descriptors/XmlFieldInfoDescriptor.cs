using FileHelpers.Core.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileHelpers.Fluent.Xml.Descriptors
{
    public class XmlFieldInfoDescriptor : FieldInfoDescriptor, IXmlFieldInfoDescriptor
    {
        public bool IsAttribute { get; set; }

        public string PropertyName { get; set; }
    }
}
