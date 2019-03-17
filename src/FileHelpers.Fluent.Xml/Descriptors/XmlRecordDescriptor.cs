using FileHelpers.Core.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileHelpers.Fluent.Xml.Descriptors
{
    public class XmlRecordDescriptor : RecordDescriptor
    {
        public string RootElementName { get; set; }

        public string ElementName { get; set; }
    }
}
