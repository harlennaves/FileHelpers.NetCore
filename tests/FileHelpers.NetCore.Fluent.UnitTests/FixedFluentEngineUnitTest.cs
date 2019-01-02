using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

using FileHelpers.NetCore.Converters;
using FileHelpers.NetCore.Fluent.Builders;
using FileHelpers.NetCore.Fluent.Engines;
using FileHelpers.NetCore.Fluent.Exceptions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
// ReSharper disable UnusedVariable

namespace FileHelpers.NetCore.Fluent.UnitTests
{
    [TestClass]
    public class FixedFluentEngineUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Has_No_Fields()
        {
            var builder = new FluentFixedBuilder();

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Property_Name()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("")
                   .SetLength(10);

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Property_Length()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Name");

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Array_Lenght()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Function")
                   .SetLength(3)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0');

            builder.Add("ArraySize")
                   .SetLength(4)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0')
                   .SetConverter(typeof(IntegerConverter));

            var arrayBuilder = builder.AddArray("ArrayData");
            arrayBuilder
                        .SetArrayItemLength(10)
                        .SetAlign(true)
                        .SetResidualAlignChar('0');

            arrayBuilder.Add("DealId")
                        .SetNullValue(string.Empty)
                        .SetLength(10)
                        .SetAlign(AlignMode.Right)
                        .SetAlignChar('0');

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_ArrayItem_Lenght()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Function")
                   .SetLength(3)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0');

            builder.Add("ArraySize")
                   .SetLength(4)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0')
                   .SetConverter(typeof(IntegerConverter));

            var arrayBuilder = builder.AddArray("ArrayData");
            arrayBuilder
                .SetArrayLength(500)
                .SetAlign(true)
                .SetResidualAlignChar('0');

            arrayBuilder.Add("DealId")
                        .SetNullValue(string.Empty)
                        .SetLength(10)
                        .SetAlign(AlignMode.Right)
                        .SetAlignChar('0');

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Array_Remainder_Length()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Function")
                   .SetLength(3)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0');

            builder.Add("ArraySize")
                   .SetLength(4)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0')
                   .SetConverter(typeof(IntegerConverter));

            var arrayBuilder = builder.AddArray("ArrayData");
            arrayBuilder
                .SetArrayLength(500)
                .SetArrayLength(11)
                .SetAlign(true)
                .SetResidualAlignChar('0');

            arrayBuilder.Add("DealId")
                        .SetNullValue(string.Empty)
                        .SetLength(10)
                        .SetAlign(AlignMode.Right)
                        .SetAlignChar('0');

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_ArrayItem_Greater_Than_Array()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Function")
                   .SetLength(3)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0');

            builder.Add("ArraySize")
                   .SetLength(4)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0')
                   .SetConverter(typeof(IntegerConverter));

            var arrayBuilder = builder.AddArray("ArrayData");
            arrayBuilder
                .SetArrayLength(5)
                .SetArrayLength(10)
                .SetAlign(true)
                .SetResidualAlignChar('0');

            arrayBuilder.Add("DealId")
                        .SetNullValue(string.Empty)
                        .SetLength(10)
                        .SetAlign(AlignMode.Right)
                        .SetAlignChar('0');

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Array_Without_Fields()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Function")
                   .SetLength(3)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0');

            builder.Add("ArraySize")
                   .SetLength(4)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0')
                   .SetConverter(typeof(IntegerConverter));

            var arrayBuilder = builder.AddArray("ArrayData");
            arrayBuilder
                .SetArrayLength(500)
                .SetArrayLength(10)
                .SetAlign(true)
                .SetResidualAlignChar('0');

            var engine = new FluentFileHelperEngine(builder);
        }

        [TestMethod]
        public void Fluent_Write()
        {
            var fluentBuilder = new FluentFixedBuilder()
            {
                FixedMode = FixedMode.ExactLength
            };
            fluentBuilder.Add("Name")
                         .SetLength(50);

            fluentBuilder.Add("Doc")
                         .SetLength(14)
                         .SetAlign(AlignMode.Left)
                         .SetAlignChar('0');

            var fluentEngine = new FluentFileHelperEngine(fluentBuilder);

            ExpandoObject item = new ExpandoObject();
            item.TryAdd("Name", "Harlen Naves");
            item.TryAdd("Doc", 05870659663);

            string line = fluentEngine.WriteString(new[] { item });
        }

        [TestMethod]
        public void Fluent_Write_With_DateTime_yyyyMMdd_Converter()
        {
            var builder = new FluentFixedBuilder
            {
                FixedMode = FixedMode.ExactLength
            };

            builder.Add("Name")
                   .SetLength(50);

            builder.Add("Doc")
                   .SetLength(14)
                   .SetAlign(AlignMode.Left)
                   .SetAlignChar('0');

            builder.Add("BirthDate")
                   .SetLength(8)
                   .SetConverter(typeof(DateTimeConverter))
                   .SetConverterFormat("yyyyMMdd");

            var fluentEngine = new FluentFileHelperEngine(builder);

            var item = new ExpandoObject();
            item.TryAdd("Name", "Harlen Naves");
            item.TryAdd("Doc", 05870659663);
            item.TryAdd("BirthDate", new DateTime(1984, 03, 30));

            string line = fluentEngine.WriteString(new[] { item });

            Assert.AreEqual("Harlen Naves                                      0000587065966319840330\r\n", line);
        }

        [TestMethod]
        public void Fluent_Read_With_DateTime_yyyyMMdd_Converter()
        {
            var builder = new FluentFixedBuilder
            {
                FixedMode = FixedMode.ExactLength
            };

            builder.Add("Name")
                   .SetTrimMode(TrimMode.Both)
                   .SetLength(50);

            builder.Add("Doc")
                   .SetLength(14)
                   .SetAlign(AlignMode.Left)
                   .SetConverter(typeof(LongConverter))
                   .SetAlignChar('0');

            builder.Add("BirthDate")
                   .SetLength(8)
                   .SetConverter(typeof(DateTimeConverter))
                   .SetConverterFormat("yyyyMMdd");

            var fluentEngine = new FluentFileHelperEngine(builder);

            var items = fluentEngine.ReadString("Harlen Naves                                      0000587065966319840330");

            Assert.AreEqual(1, items.Length);

            dynamic item = items[0];

            Assert.AreEqual("Harlen Naves", item.Name);
            Assert.AreEqual(05870659663, item.Doc);
        }

        [TestMethod]
        public void Fluent_Read_With_Array()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Function")
                   .SetLength(3)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0');

            builder.Add("ArraySize")
                   .SetLength(4)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0')
                   .SetConverter(typeof(IntegerConverter));

            var arrayBuilder = builder.AddArray("ArrayData");
            arrayBuilder.SetArrayLength(500)
                        .SetArrayItemLength(10)
                        .SetAlign(true)
                        .SetResidualAlignChar('0');

            arrayBuilder.Add("DealId")
                        .SetNullValue(string.Empty)
                        .SetLength(10)
                        .SetAlign(AlignMode.Right)
                        .SetAlignChar('0');

            var engine = new FluentFileHelperEngine(builder);

            var items = engine.ReadString(
                "0010025STOCKAPA17STOCKASS18STOCKASS17STOCKDIC18STOCKDIC17STOCKDIP18STOCKDIP17STOCKGOT18STOCKGOT17STOCKLUV18STOCKLUV17STOCKNOR18STOCKNOR17STOCKRIV18STOCKRIV17STOCKSUE18STOCKSUE17STOCKTRC18STOCKTRC17STOCKTRV18STOCKTRV17STOCKLAP18STOCKLAP17STOCKASC18STOCKASC17000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
        }

        [TestMethod]
        public void Fluent_Write_With_Array()
        {
            var builder = new FluentFixedBuilder();

            builder.Add("Function")
                   .SetLength(3)
                   .SetAlign(AlignMode.Right)
                   .SetAlignChar('0');

            builder.Add("ArraySize")
                   .SetLength(4)
                   .SetAlign(AlignMode.Left)
                   .SetAlignChar('0')
                   .SetConverter(typeof(IntegerConverter));

            var arrayBuilder = builder.AddArray("ArrayData");
            arrayBuilder.SetArrayLength(500)
                        .SetArrayItemLength(10)
                        .SetAlign(true)
                        .SetResidualAlignChar('0');

            arrayBuilder.Add("DealId")
                        .SetNullValue(string.Empty)
                        .SetLength(10)
                        .SetAlign(AlignMode.Right)
                        .SetAlignChar('0');

            var engine = new FluentFileHelperEngine(builder);

            ExpandoObject item = new ExpandoObject();
            item.TryAdd("Function", "001");
            item.TryAdd("ArraySize", 25);

            List<ExpandoObject> arrayData = new List<ExpandoObject>();
            for (int i = 0; i < 25; i++)
            {
                ExpandoObject arrayItem = new ExpandoObject();
                arrayItem.TryAdd("DealId", "STOCKASC" + i.ToString().PadLeft(2, '0'));
                arrayData.Add(arrayItem);
            }

            item.TryAdd("ArrayData", arrayData);

            string content = engine.WriteString(new[] { item });
        }

        [TestMethod]
        public void Parse_Json_Config()
        {
            string jsonStr = "{\"FixedMode\" : 0,\"IgnoreEmptyLines\" : true,\"Fields\" : [{\"Name\" : \"Function\", \"IsArray\" : false,\"Length\" : 3,\"Align\" : 2,\"AlignChar\" : \"0\"},{\"Name\" : \"ArraySize\",\"Length\" : 4,\"Align\" : 0,\"AlignChar\" : \"0\",\"Converter\" : \"FileHelpers.NetCore.Converters.IntegerConverter, FileHelpers.NetCore.Fluent\"},{ \"Name\": \"ArrayData\", \"IsArray\" : true,\"ArrayLength\" : 500,\"ArrayItemLength\" : 10,\"Align\" : true,\"AlignChar\" : \"0\",\"Fields\" : [{\"Name\" : \"DealId\",\"NullValue\" : \"\",\"Length\" : 10,\"Align\" : 2,\"AlignChar\": \"0\"}]}]}";
            
            var engine = new FluentFileHelperEngine(jsonStr);
            
            ExpandoObject item = new ExpandoObject();
            item.TryAdd("Function", "001");
            item.TryAdd("ArraySize", 25);

            List<ExpandoObject> arrayData = new List<ExpandoObject>();
            for (int i = 0; i < 25; i++)
            {
                ExpandoObject arrayItem = new ExpandoObject();
                arrayItem.TryAdd("DealId", "STOCKASC" + i.ToString().PadLeft(2, '0'));
                arrayData.Add(arrayItem);
            }

            item.TryAdd("ArrayData", arrayData);

            string content = engine.WriteString(new[] { item });
        }
    }
}
