using System;
using System.Collections.Generic;
using System.Dynamic;

using FileHelpers.NetCore.Core.Converters;
using FileHelpers.NetCore.Fluent.Exceptions;
using FileHelpers.NetCore.Fluent.Fixed.Descriptors;
using FileHelpers.NetCore.Fluent.Fixed.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileHelpers.NetCore.Fluent.Fixed.UnitTests
{
    [TestClass]
    public class FixedFluentEntineUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Has_No_Fields()
        {
            var descriptor = new FixedRecordDescriptor();

            var engine = new FluentFixedEngine(descriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Property_Length()
        {
            var descriptor = new FixedRecordDescriptor();
            descriptor.AddField("")
                      .SetLength(10);
            var engine = new FluentFixedEngine(descriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Array_Length()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3);

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            var arrayBuilder = descriptor.AddArray("ArrayData");
            arrayBuilder.SetArrayItemLength(10)
                        .SetAlign(true);

            arrayBuilder.AddField("DealId")
                        .SetLength(10)
                        .SetAlignMode(AlignMode.Right)
                        .SetNullValue(string.Empty)
                        .SetAlignChar('0');

            var engine = new FluentFixedEngine(descriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_ArrayItem_Length()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3);

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            var arrayBuilder = descriptor.AddArray("ArrayData");
            arrayBuilder.SetArrayLength(500)
                        .SetAlign(true);

            arrayBuilder.AddField("DealId")
                        .SetLength(10)
                        .SetAlignMode(AlignMode.Right)
                        .SetNullValue(string.Empty)
                        .SetAlignChar('0');

            var engine = new FluentFixedEngine(descriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Array_Remainder_Length()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3);

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            var arrayBuilder = descriptor.AddArray("ArrayData");
            arrayBuilder.SetArrayLength(500)
                        .SetArrayItemLength(11)
                        .SetAlign(true);

            arrayBuilder.AddField("DealId")
                        .SetLength(10)
                        .SetAlignMode(AlignMode.Right)
                        .SetNullValue(string.Empty)
                        .SetAlignChar('0');

            var engine = new FluentFixedEngine(descriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_ArrayItem_Greater_Than_Array()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3);

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            var arrayBuilder = descriptor.AddArray("ArrayData");
            arrayBuilder.SetArrayLength(500)
                        .SetArrayItemLength(501)
                        .SetAlign(true);

            arrayBuilder.AddField("DealId")
                        .SetLength(10)
                        .SetAlignMode(AlignMode.Right)
                        .SetNullValue(string.Empty)
                        .SetAlignChar('0');

            var engine = new FluentFixedEngine(descriptor);
        }

        [TestMethod]
        [ExpectedException(typeof(BadFluentConfigurationException))]
        public void Invalid_Array_Without_Fields()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3);

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            var arrayBuilder = descriptor.AddArray("ArrayData");
            arrayBuilder.SetArrayLength(500)
                        .SetArrayItemLength(10)
                        .SetAlign(true);

            var engine = new FluentFixedEngine(descriptor);
        }

        [TestMethod]
        public void Fluent_Write()
        {
            var recordDescriptor = new FixedRecordDescriptor();
            recordDescriptor.AddField("Name")
                            .SetLength(50);
            recordDescriptor.AddField("Doc")
                            .SetLength(14)
                            .SetAlignMode(AlignMode.Left)
                            .SetAlignChar('0');

            var engine = new FluentFixedEngine(recordDescriptor);

            ExpandoObject item = new ExpandoObject();
            item.TryAdd("Name", "Harlen Naves");
            item.TryAdd("Doc", 05870659663);

            string line = engine.WriteString(new[] { item });
        }

        [TestMethod]
        public void Fluent_Write_With_DateTime_yyyyMMdd_Converter()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(50)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Doc")
                      .SetLength(14)
                      .SetAlignMode(AlignMode.Left)
                      .SetAlignChar('0');

            descriptor.AddField("BirthDate")
                      .SetLength(8)
                      .SetConverter(typeof(DateTimeConverter))
                      .SetConverterFormat("yyyyMMdd");

            var engine = new FluentFixedEngine(descriptor);

            var item = new ExpandoObject();
            item.TryAdd("Name", "Harlen Naves");
            item.TryAdd("Doc", 05870659663);
            item.TryAdd("BirthDate", new DateTime(1984, 03, 30));

            string line = engine.WriteString(new[] { item });

            Assert.AreEqual("Harlen Naves                                      0000587065966319840330" + Environment.NewLine, line);
        }

        [TestMethod]
        public void Fluent_Read_With_DateTime_yyyyMMdd_Converter()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(50)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Doc")
                      .SetLength(14)
                      .SetAlignMode(AlignMode.Left)
                      .SetConverter(typeof(LongConverter))
                      .SetAlignChar('0');

            descriptor.AddField("BirthDate")
                      .SetLength(8)
                      .SetConverter(typeof(DateTimeConverter))
                      .SetConverterFormat("yyyyMMdd");

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString("Harlen Naves                                      0000587065966319840330");

            Assert.AreEqual(1, items.Length);

            dynamic item = items[0];

            Assert.AreEqual("Harlen Naves", item.Name);
            Assert.AreEqual(05870659663, item.Doc);
        }

        [TestMethod]
        public void Fluent_Read_With_Array()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Left)
                      .SetAlignChar('0')
                      .SetConverter(typeof(IntegerConverter));

            var arrayDescriptor = descriptor.AddArray("ArrayData")
                                            .SetArrayLength(500)
                                            .SetArrayItemLength(10)
                                            .SetAlign(true)
                                            .SetAlignChar('0');

            arrayDescriptor.AddField("DealId")
                           .SetLength(10)
                           .SetNullValue(string.Empty)
                           .SetAlignMode(AlignMode.Right)
                           .SetAlignChar('0');

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString(
                "0010025STOCKAPA17STOCKASS18STOCKASS17STOCKDIC18STOCKDIC17STOCKDIP18STOCKDIP17STOCKGOT18STOCKGOT17STOCKLUV18STOCKLUV17STOCKNOR18STOCKNOR17STOCKRIV18STOCKRIV17STOCKSUE18STOCKSUE17STOCKTRC18STOCKTRC17STOCKTRV18STOCKTRV17STOCKLAP18STOCKLAP17STOCKASC18STOCKASC17000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");

            Assert.AreEqual(1, items.Length);

            dynamic item = items[0];

            Assert.AreEqual("001", item.Function);
            Assert.AreEqual(25, item.ArraySize);
            Assert.AreEqual(item.ArraySize, item.ArrayData.Length);
        }

        [TestMethod]
        public void Fluent_Write_With_Array()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Left)
                      .SetAlignChar('0')
                      .SetConverter(typeof(IntegerConverter));

            var arrayDescriptor = descriptor.AddArray("ArrayData")
                                            .SetArrayLength(500)
                                            .SetArrayItemLength(10)
                                            .SetAlign(true)
                                            .SetAlignChar('0');

            arrayDescriptor.AddField("DealId")
                           .SetLength(10)
                           .SetNullValue(string.Empty)
                           .SetAlignMode(AlignMode.Right)
                           .SetAlignChar('0');

            FluentFixedEngine engine = descriptor.Build();

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

            Assert.AreEqual("0010025STOCKASC00STOCKASC01STOCKASC02STOCKASC03STOCKASC04STOCKASC05STOCKASC06STOCKASC07STOCKASC08STOCKASC09STOCKASC10STOCKASC11STOCKASC12STOCKASC13STOCKASC14STOCKASC15STOCKASC16STOCKASC17STOCKASC18STOCKASC19STOCKASC20STOCKASC21STOCKASC22STOCKASC23STOCKASC240000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" + Environment.NewLine, content);
        }

        [TestMethod]
        public void Fluent_Write_With_Array_With_No_Align()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3)
                      .SetAlignMode(AlignMode.Right)
                      .SetAlignChar('0');

            descriptor.AddField("ArraySize")
                      .SetLength(4)
                      .SetAlignMode(AlignMode.Left)
                      .SetAlignChar('0')
                      .SetConverter(typeof(IntegerConverter));

            var arrayDescriptor = descriptor.AddArray("ArrayData")
                                            .SetArrayLength(500)
                                            .SetArrayItemLength(10)
                                            .SetAlign(false);

            arrayDescriptor.AddField("DealId")
                           .SetLength(10)
                           .SetNullValue(string.Empty)
                           .SetAlignMode(AlignMode.Right)
                           .SetAlignChar('0');

            FluentFixedEngine engine = descriptor.Build();

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

            Assert.AreEqual("0010025STOCKASC00STOCKASC01STOCKASC02STOCKASC03STOCKASC04STOCKASC05STOCKASC06STOCKASC07STOCKASC08STOCKASC09STOCKASC10STOCKASC11STOCKASC12STOCKASC13STOCKASC14STOCKASC15STOCKASC16STOCKASC17STOCKASC18STOCKASC19STOCKASC20STOCKASC21STOCKASC22STOCKASC23STOCKASC24" + Environment.NewLine, content);
        }
    }
}
