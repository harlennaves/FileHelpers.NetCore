using FileHelpers.Fluent.Xml.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FileHelpers.Fluent.Extensions;
using FileHelpers.Core.Converters;
using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Exceptions;
using FileHelpers.Fluent.Xml.Builders;

namespace FileHelpers.Fluent.Xml.Extensions
{
    public static class XmlFiledBuilderDescriptorExtensions
    {

        public static XmlFieldInfoBuilder AddField(this IRecordDescriptor recordDescriptor, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                throw new BadFluentConfigurationException($"The {nameof(fieldName)} cannot be null or empty");

            var fieldInfo = new XmlFieldInfoBuilder();
            recordDescriptor.Add(fieldName, fieldInfo);

            return fieldInfo;
        }

        public static object ToRecord(this IXmlFieldInfoDescriptor fieldDescriptor, string name, XElement element)
        {
            if (fieldDescriptor.IsAttribute)
                return fieldDescriptor.ParseAttribute(name, element);
            
            return fieldDescriptor.ParseElement(name, element);
        }

        public static void RecordToXml(this IXmlFieldInfoDescriptor fieldDescriptor, string propertyName, XElement parent, object record)
        {
            if (fieldDescriptor.IsAttribute)
            {
                parent.SetAttributeValue(propertyName, fieldDescriptor.CreateFieldString(record));
                return;
            }
            parent.SetElementValue(propertyName, fieldDescriptor.CreateFieldString(record));
        }

        private static string CreateFieldString(this IXmlFieldInfoDescriptor fieldDescriptor, object fieldValue)
        {
            if (fieldDescriptor.Converter == null)
            {
                if (fieldValue == null)
                    return string.Empty;
                return fieldValue.ToString();
            }

            ConverterBase converterInstance =
                ConverterFactory.GetConverter(fieldDescriptor.Converter, fieldDescriptor.ConverterFormat);

            return converterInstance?.FieldToString(fieldValue) ?? string.Empty;
        }

        private static object ParseAttribute(this IXmlFieldInfoDescriptor fieldDescriptor, string attributeName, XElement element)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute == null)
                return null;

            return fieldDescriptor.StringToField(attribute.Value);
        }

        private static object ParseElement(this IXmlFieldInfoDescriptor fieldDescriptor, string elementName, XElement element)
        {
            
            XElement elementItem = null;
            foreach (XElement childElement in element.Elements())
            {
                if (childElement.Name.LocalName != elementName)
                    continue;

                elementItem = childElement;
                break;
            }

            if (elementItem == null || elementItem.Value == null)
                return null;

            return fieldDescriptor.StringToField(elementItem.Value);
        }
    }
}
