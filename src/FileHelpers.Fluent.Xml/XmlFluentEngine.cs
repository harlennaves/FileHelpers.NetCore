using FileHelpers.Core.Descriptors;
using FileHelpers.Core.Engines;
using FileHelpers.Fluent.Exceptions;
using FileHelpers.Fluent.Xml.Descriptors;
using FileHelpers.Fluent.Xml.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileHelpers.Fluent.Xml
{
    public class XmlFluentEngine : FluentEngineBase
    {
        public XmlFluentEngine(IRecordDescriptor descriptor) : base(descriptor)
        {
        }

        public XmlFluentEngine(IRecordDescriptor descriptor, Encoding encoding) : base(descriptor, encoding)
        {
        }

        public override ExpandoObject[] ReadFile(string fileName) =>
            ReadFileAsync(fileName).GetAwaiter().GetResult();

        public override async Task<ExpandoObject[]> ReadFileAsync(string fileName)
        {
            using (var reader = new StreamReader(fileName))
                return await ReadStreamAsync(reader);
        }

        public override ExpandoObject[] ReadBuffer(byte[] buffer) =>
            ReadBufferAsync(buffer).GetAwaiter().GetResult();

        public override Task<ExpandoObject[]> ReadBufferAsync(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            using (var streamReader = new StreamReader(stream, Encoding))
                return ReadStreamAsync(streamReader);
        }

        public override ExpandoObject[] ReadStream(StreamReader reader) =>
            ReadStreamAsync(reader).GetAwaiter().GetResult();

        public override async Task<ExpandoObject[]> ReadStreamAsync(StreamReader reader)
        {
            IList<ExpandoObject> items = new List<ExpandoObject>();
            var cancellationToken = new CancellationTokenSource();
            var xmlDocument = await XDocument.LoadAsync(reader, LoadOptions.None, cancellationToken.Token);

            IEnumerable<XElement> elements = null;

            string rootElementName = ((XmlRecordDescriptor)Descriptor).RootElementName;
            if (string.IsNullOrWhiteSpace(rootElementName))
                elements = xmlDocument.Elements();
            else
            {
                var firstElement = xmlDocument.Elements().FirstOrDefault();
                elements = (firstElement == null || firstElement.Name.LocalName != rootElementName)
                    ? xmlDocument.Elements()
                    : firstElement.Elements();
            }

            foreach (XElement element in elements)
            {
                items.Add(ReadElement(element, Descriptor));
            }

            return items.ToArray();
        }

        public override ExpandoObject[] ReadString(string source)
        {
            if (source == null)
                source = string.Empty;
            using (var stream = new MemoryStream(Encoding.GetBytes(source)))
            {
                using (var streamReader = new StreamReader(stream))
                    return ReadStream(streamReader);
            }
        }

        private ExpandoObject ReadElement(XElement element, IRecordDescriptor descriptor)
        {
            var item = new ExpandoObject();

            foreach (KeyValuePair<string, IFieldInfoTypeDescriptor> fieldInfoTypeDescriptor in descriptor.Fields)
            {
                string propertyName = string.IsNullOrWhiteSpace(((IXmlFieldPropertyNameInfoDescriptor)fieldInfoTypeDescriptor.Value).PropertyName)
                            ? fieldInfoTypeDescriptor.Key
                            : ((IXmlFieldPropertyNameInfoDescriptor)fieldInfoTypeDescriptor.Value).PropertyName;
                
                if (fieldInfoTypeDescriptor.Value.IsArray)
                {
                    item.TryAdd(propertyName,
                    ((IXmlArrayFieldInfoDescriptor)fieldInfoTypeDescriptor.Value).ToRecordArray(
                            fieldInfoTypeDescriptor.Key,
                            element
                        ));
                    continue;
                }

                item.TryAdd(propertyName,
                        ((IXmlFieldInfoDescriptor)fieldInfoTypeDescriptor.Value).ToRecord(fieldInfoTypeDescriptor.Key, element)
                    );
            }

            return item;
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }

        public override void WriteFile(string fileName, IEnumerable<ExpandoObject> records) => 
            WriteFileAsync(fileName, records).GetAwaiter().GetResult();

        public override async Task WriteFileAsync(string fileName, IEnumerable<ExpandoObject> records)
        {
            using(var writer = new StreamWriter(fileName))
                await WriteStreamAsync(writer, records);
        }

        public override void WriteStream(TextWriter writer, IEnumerable<ExpandoObject> records, bool flush = true) =>
            WriteStreamAsync(writer, records, flush).GetAwaiter().GetResult();

        public override async Task WriteStreamAsync(TextWriter writer, IEnumerable<ExpandoObject> records, bool flush = true)
        {
            var xmlDescriptor = ((XmlRecordDescriptor)Descriptor);
            string rootElementName = xmlDescriptor.RootElementName;
            string elementName = xmlDescriptor.ElementName;

            if (records.Count() > 1 && rootElementName == elementName)
                throw new BadFluentConfigurationException("There are more than one record without root element. It is impossible to create a valid XML.");

            XDocument xmlDocument = new XDocument();
            
            XElement rootElement = new XElement(rootElementName);
            xmlDocument.Add(rootElement);
            foreach (ExpandoObject record in records)
            {
                XElement element;
                if (rootElementName == elementName)
                    element = rootElement;
                else
                {
                    element = new XElement(elementName);
                    rootElement.Add(element);
                }
                foreach(KeyValuePair<string, object> keyValuePair in record)
                {
                    string propertyName = keyValuePair.Key;
                    if (!Descriptor.Fields.TryGetValue(keyValuePair.Key, out IFieldInfoTypeDescriptor fieldDescriptor))
                    {
                        fieldDescriptor = Descriptor.Fields.Values.FirstOrDefault(x => ((IXmlFieldPropertyNameInfoDescriptor)x).PropertyName == keyValuePair.Key);
                        if (fieldDescriptor == null)
                            throw new Exception($"The field {keyValuePair.Key} is not configured"); 
                    }
                    
                    if (fieldDescriptor.IsArray)
                    {
                        ((IXmlArrayFieldInfoDescriptor)fieldDescriptor).ArrayToXml(propertyName, element, (IEnumerable<dynamic>)keyValuePair.Value);
                        continue;
                    }
                    ((IXmlFieldInfoDescriptor)fieldDescriptor).RecordToXml(propertyName, element, keyValuePair.Value);
                }

            }

            await xmlDocument.SaveAsync(writer, SaveOptions.DisableFormatting, new CancellationToken());
        }

        public override string WriteString(IEnumerable<ExpandoObject> records)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                WriteStream(writer, records);
                return sb.ToString();
            }
        }

        protected override void CheckFieldDescriptor(string fieldName, IFieldInfoTypeDescriptor fieldDescriptor)
        {
            return;
        }

        protected override Task<ExpandoObject> ReadLineAsync(string currentLine, IRecordDescriptor descriptor)
        {
            throw new NotImplementedException();
        }
    }
}
