using FileHelpers.Core.Descriptors;
using FileHelpers.Core.Engines;
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
using System.Xml;
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

        public override ExpandoObject[] ReadStream(StreamReader reader) =>
            ReadStreamAsync(reader).GetAwaiter().GetResult();

        public override async Task<ExpandoObject[]> ReadStreamAsync(StreamReader reader)
        {
            IList<ExpandoObject> items = new List<ExpandoObject>();
            var cancellationToken = new CancellationTokenSource();
            var xmlDocument = await XDocument.LoadAsync(reader, LoadOptions.None, cancellationToken.Token);

            foreach (XElement element in xmlDocument.Elements())
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
                if (fieldInfoTypeDescriptor.Value.IsArray)
                {
                    continue;
                }
                var fieldInfoDescriptor = (IXmlFieldInfoDescriptor)fieldInfoTypeDescriptor.Value;
                item.TryAdd(string.IsNullOrWhiteSpace(fieldInfoDescriptor.PropertyName) ? fieldInfoTypeDescriptor.Key : fieldInfoDescriptor.PropertyName,
                    ((IXmlFieldInfoDescriptor)fieldInfoTypeDescriptor.Value).ToRecord(fieldInfoTypeDescriptor.Key, element)
                    );
            }

            return item;
        }

        public override string Serialize()
        {
            throw new NotImplementedException();
        }

        public override void WriteFile(string fileName, IEnumerable<ExpandoObject> records)
        {
            throw new NotImplementedException();
        }

        public override Task WriteFileAsync(string fileName, IEnumerable<ExpandoObject> records)
        {
            throw new NotImplementedException();
        }

        public override void WriteStream(TextWriter writer, IEnumerable<ExpandoObject> records)
        {
            throw new NotImplementedException();
        }

        public override Task WriteStreamAsync(TextWriter writer, IEnumerable<ExpandoObject> records)
        {
            throw new NotImplementedException();
        }

        public override string WriteString(IEnumerable<ExpandoObject> records)
        {
            throw new NotImplementedException();
        }

        protected override void CheckFieldDescriptor(string fieldName, IFieldInfoTypeDescriptor fieldDescriptor)
        {
            return;
        }

        protected override Task<ExpandoObject> ReadLineAsync(string currentLine, IRecordDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        private void Parse(dynamic parent, XElement node)
        {
            try
            {
                if (node.HasElements)
                {
                    if (node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
                    {
                        //list
                        var item = new ExpandoObject();
                        var list = new List<dynamic>();
                        foreach (var element in node.Elements())
                        {
                            Parse(list, element);
                        }

                        AddProperty(item, node.Elements().First().Name.LocalName, list);
                        AddProperty(parent, node.Name.ToString(), item);
                    }
                    else
                    {
                        var item = new ExpandoObject();

                        foreach (var attribute in node.Attributes())
                        {
                            AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
                        }

                        //element
                        foreach (var element in node.Elements())
                        {
                            Parse(item, element);
                        }

                        AddProperty(parent, node.Name.ToString(), item);
                    }
                }
                else
                {
                    AddProperty(parent, node.Name.ToString(), node.Value.Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AddProperty(dynamic parent, string name, object value)
        {
            try
            {
                if (parent is List<dynamic>)
                {
                    (parent as List<dynamic>).Add(value);
                }
                else
                {
                    (parent as IDictionary<string, object>)[name] = value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
