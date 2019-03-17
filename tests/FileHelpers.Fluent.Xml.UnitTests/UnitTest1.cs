using FileHelpers.Core.Converters;
using FileHelpers.Fluent.Xml.Descriptors;
using FileHelpers.Fluent.Xml.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FileHelpers.Fluent.Xml.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Read()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?> <mgns1:Customer AddCity=\"NO NAME\" AddComplement=\"KM 2,3\" AddNeighborhood=\"ABCDEF\" AddNumber=\"1\" AddPostalCode=\"123456\" AddState=\"SP\" AddStreet=\"ac=bcdef ghijk\" BirthDate=\"2018-12-06\" CNPJCPF=\"123456789\" CNPJOriginDealer=\"123456789\"  CdOriginDealer=\"2\" CdUser=\"S32\" Email=\"\" Phone=\"123456\" StateRegistration=\"671305412113\" Suframa=\"\" UpdateDate=\"2019-01-03\" UsualName=\"ABC DEF GHI\" xmlns:mgns1=\"http://www.volvo.com/example/part/1_3\"><Name>abc def ghi</Name></mgns1:Customer>";

            var descriptor = new XmlRecordDescriptor();

            descriptor.AddField("AddCity")
                .SetIsAttribute(true)
                .SetPropertyName("City");
                
            descriptor.AddField("AddComplement")
                .SetIsAttribute(true)
                .SetPropertyName("AddressComplement");

            descriptor.AddField("AddNeighborhood")
                .SetIsAttribute(true)
                .SetPropertyName("Neighborhood");

            descriptor.AddField("AddNumber")
                .SetIsAttribute(true)
                .SetPropertyName("AddressNumber");

            descriptor.AddField("AddPostalCode")
                .SetIsAttribute(true)
                .SetPropertyName("PostalCode");

            descriptor.AddField("AddState")
                .SetIsAttribute(true)
                .SetPropertyName("Province");

            descriptor.AddField("AddStreet")
                .SetIsAttribute(true)
                .SetPropertyName("Street");

            descriptor.AddField("BirthDate")
                .SetIsAttribute(true)
                .SetConverter(typeof(DateTimeConverter))
                .SetConverterFormat("yyyy-MM-dd");

            descriptor.AddField("CNPJCPF")
                .SetIsAttribute(true)
                .SetPropertyName("Document")
                .SetType(typeof(ulong));

            descriptor.AddField("Name")
                .SetIsAttribute(true);


            var engine = new XmlFluentEngine(descriptor);

            var items = engine.ReadString(xml);
        }

        [TestMethod]
        public void Read_With_Element()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?> <mgns1:Customer AddCity=\"NO NAME\" AddComplement=\"KM 2,3\" AddNeighborhood=\"ABCDEF\" AddNumber=\"1\" AddPostalCode=\"123456\" AddState=\"SP\" AddStreet=\"ac=bcdef ghijk\" BirthDate=\"2018-12-06\" CNPJCPF=\"123456789\" CNPJOriginDealer=\"123456789\"  CdOriginDealer=\"2\" CdUser=\"S32\" Email=\"\" Phone=\"123456\" StateRegistration=\"671305412113\" Suframa=\"\" UpdateDate=\"2019-01-03\" UsualName=\"ABC DEF GHI\" xmlns:mgns1=\"http://www.volvo.com/example/part/1_3\"><Name>abc def ghi</Name></mgns1:Customer>";

            var descriptor = new XmlRecordDescriptor();

            descriptor.AddField("AddCity")
                .SetIsAttribute(true)
                .SetPropertyName("City");

            descriptor.AddField("AddComplement")
                .SetIsAttribute(true)
                .SetPropertyName("AddressComplement");

            descriptor.AddField("AddNeighborhood")
                .SetIsAttribute(true)
                .SetPropertyName("Neighborhood");

            descriptor.AddField("AddNumber")
                .SetIsAttribute(true)
                .SetPropertyName("AddressNumber");

            descriptor.AddField("AddPostalCode")
                .SetIsAttribute(true)
                .SetPropertyName("PostalCode");

            descriptor.AddField("AddState")
                .SetIsAttribute(true)
                .SetPropertyName("Province");

            descriptor.AddField("AddStreet")
                .SetIsAttribute(true)
                .SetPropertyName("Street");

            descriptor.AddField("BirthDate")
                .SetIsAttribute(true)
                .SetConverter(typeof(DateTimeConverter))
                .SetConverterFormat("yyyy-MM-dd");

            descriptor.AddField("CNPJCPF")
                .SetIsAttribute(true)
                .SetPropertyName("Document")
                .SetType(typeof(ulong));

            descriptor.AddField("Name");


            var engine = new XmlFluentEngine(descriptor);

            var items = engine.ReadString(xml);
        }

        [TestMethod]
        public void Read_Array()
        {
            var descriptor = new XmlRecordDescriptor 
            {
                RootElementName = "Clients",
                ElementName = "Client"
            };

            descriptor.AddField("Name");
            descriptor.AddField("Document")
                .SetIsAttribute(true);
            var addressesDescriptor = descriptor.AddArray("Addresses");
            addressesDescriptor.AddField("Street")
                .SetIsAttribute(true);
            addressesDescriptor.AddField("Number")
                .SetIsAttribute(true)
                .SetType(typeof(int));

            var engine = new XmlFluentEngine(descriptor);

            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><Clients><Client Document=\"987654321\"><Name>No name 1</Name><Addresses><Address Street=\"First Street\" Number=\"1\"/><Address Street=\"Second Street\" Number=\"1\"/><Address Street=\"Third street\" Number=\"1\"/></Addresses></Client><Client Document=\"123456789\"><Name>No name 2</Name><Addresses><Address Street=\"First Street\" Number=\"1\"/><Address Street=\"Second Street\" Number=\"1\"/><Address Street=\"Third street\" Number=\"1\"/></Addresses></Client></Clients>";

            var items = engine.ReadString(xml);
        }

        [TestMethod]
        public void Write_Xml_With_Array()
        {
            var descriptor = new XmlRecordDescriptor
            {
                RootElementName = "Clients",
                ElementName = "Client"
            };

            descriptor.AddField("Name");
            descriptor.AddField("Document")
                .SetIsAttribute(true);
            var addressesDescriptor = descriptor.AddArray("Addresses")
                .SetElementName("Address");
            addressesDescriptor.AddField("Street")
                .SetIsAttribute(true);
            addressesDescriptor.AddField("Number")
                .SetIsAttribute(true)
                .SetType(typeof(int));

            var engine = new XmlFluentEngine(descriptor);

            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><Clients><Client Document=\"987654321\"><Name>No name 1</Name><Addresses><Address Street=\"First Street\" Number=\"1\"/><Address Street=\"Second Street\" Number=\"1\"/><Address Street=\"Third street\" Number=\"1\"/></Addresses></Client><Client Document=\"123456789\"><Name>No name 2</Name><Addresses><Address Street=\"First Street\" Number=\"1\"/><Address Street=\"Second Street\" Number=\"1\"/><Address Street=\"Third street\" Number=\"1\"/></Addresses></Client></Clients>";

            var items = engine.ReadString(xml);

            var xmlOutput = engine.WriteString(items);
        }
    }
}
