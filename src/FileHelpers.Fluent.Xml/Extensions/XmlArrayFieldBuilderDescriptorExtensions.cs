using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Exceptions;
using FileHelpers.Fluent.Xml.Builders;
using FileHelpers.Fluent.Xml.Descriptors;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FileHelpers.Fluent.Xml.Extensions
{
    public static class XmlArrayFieldBuilderDescriptorExtensions
    {
        public static IXmlArrayFieldInfoDescriptor AddArray(this IRecordDescriptor descriptor, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new BadFluentConfigurationException($"The {nameof(fieldName)} cannot be null or empty");

            var fieldInfo = new XmlArrayFieldInfoBuilder();
            descriptor.Add(fieldName, fieldInfo);

            return fieldInfo;
        }
        
        public static IXmlArrayFieldInfoDescriptor SetElementName(this IXmlArrayFieldInfoDescriptor descriptor, string elementName)
        {
            descriptor.ElementName = elementName;
            return descriptor;
        }

        public static ExpandoObject[] ToRecordArray(this IXmlArrayFieldInfoDescriptor fieldDescriptor, string name, XElement element)
        {
            IList<ExpandoObject> items = new List<ExpandoObject>();

            if (!element.HasElements)
                return items.ToArray();

            XElement elementItem = null;
            foreach (XElement childElement in element.Elements())
            {
                if (childElement.Name.LocalName != name)
                    continue;

                elementItem = childElement;
                break;
            }

            if (elementItem == null || elementItem.Value == null)
                return null;
            
            foreach (var elementArrayItem in elementItem.Elements())
            {
                var item = new ExpandoObject();
                foreach (KeyValuePair<string, IFieldInfoTypeDescriptor> fieldInfoTypeDescriptor in fieldDescriptor.Fields)
                {
                    
                    string propertyName = string.IsNullOrWhiteSpace(((IXmlFieldPropertyNameInfoDescriptor)fieldInfoTypeDescriptor.Value).PropertyName)
                                ? fieldInfoTypeDescriptor.Key
                                : ((IXmlFieldPropertyNameInfoDescriptor)fieldInfoTypeDescriptor.Value).PropertyName;

                    if (fieldInfoTypeDescriptor.Value.IsArray)
                    {
                        ((IXmlArrayFieldInfoDescriptor)fieldInfoTypeDescriptor.Value).ToRecordArray(
                                propertyName,
                                elementArrayItem
                            );
                        continue;
                    }

                    item.TryAdd(propertyName,
                            ((IXmlFieldInfoDescriptor)fieldInfoTypeDescriptor.Value).ToRecord(fieldInfoTypeDescriptor.Key, elementArrayItem)
                        );
                }
                items.Add(item);
            }

            return items.ToArray();
        }

        public static void ArrayToXml(this IXmlArrayFieldInfoDescriptor arrayFieldDescriptor, string elementName, XElement parent, IEnumerable<dynamic> array)
        {
            XElement element = new XElement(elementName);
            parent.Add(element);
            foreach (ExpandoObject record in array)
            {
                XElement childElement = new XElement(arrayFieldDescriptor.ElementName);
                element.Add(childElement);
                
                foreach (KeyValuePair<string, object> keyValuePair in record)
                {
                    string propertyName = keyValuePair.Key;
                    if (!arrayFieldDescriptor.Fields.TryGetValue(keyValuePair.Key, out IFieldInfoTypeDescriptor fieldDescriptor))
                    {
                        fieldDescriptor = arrayFieldDescriptor.Fields.Values.FirstOrDefault(x => ((IXmlFieldPropertyNameInfoDescriptor)x).PropertyName == keyValuePair.Key);
                        if (fieldDescriptor == null)
                            throw new Exception($"The field {keyValuePair.Key} is not configured");
                    }

                    if (fieldDescriptor.IsArray)
                    {
                        ((IXmlArrayFieldInfoDescriptor)fieldDescriptor).ArrayToXml(propertyName, childElement, (IEnumerable<dynamic>)keyValuePair.Value);
                        continue;
                    }
                    ((IXmlFieldInfoDescriptor)fieldDescriptor).RecordToXml(propertyName, childElement, keyValuePair.Value);
                }
            }
        }
    }
}
