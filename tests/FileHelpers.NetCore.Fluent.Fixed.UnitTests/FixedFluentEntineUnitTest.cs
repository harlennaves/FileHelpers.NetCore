using System;
using System.Collections.Generic;
using System.Dynamic;

using FileHelpers.Core.Converters;
using FileHelpers.Fluent;
using FileHelpers.Fluent.Exceptions;
using FileHelpers.Fluent.Fixed;
using FileHelpers.Fluent.Fixed.Descriptors;
using FileHelpers.Fluent.Fixed.Extensions;
using FileHelpers.Fluent.Fixed.Json;

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
        public void Write()
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
        public void Write_With_DateTime_yyyyMMdd_Converter()
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
        public void Read_With_DateTime_yyyyMMdd_Converter()
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
        public void Read_With_Array()
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
        public void Write_With_Array()
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
        public void Write_With_Array_With_No_Align()
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

        [TestMethod]
        public void Serialize_Descriptor()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Function")
                      .SetLength(3)
                      .SetAlignMode(AlignMode.Right)
                      .SetType(typeof(int))
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

            var jsonDescriptor = descriptor.Build().Serialize();
        }

        [TestMethod]
        public void Deserialize_Descriptor()
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

            var engineToSerialize = descriptor.Build();

            var jsonDescriptor = engineToSerialize.Serialize();

            var engine = FluentFixedEngine.Build(jsonDescriptor);

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

        [TestMethod]
        public void Read_With_NullValue()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(50)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Doc")
                      .SetLength(14)
                      .SetAlignMode(AlignMode.Left)
                      .SetConverter(typeof(LongConverter))
                      .SetNullValue("05870659663")
                      .SetAlignChar('0');

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString("Harlen Naves                                                    ");


        }

        [TestMethod]
        public void Read_With_Decimal_Positive()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(10)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Price")
                .SetLength(18)
                .SetConverter(typeof(DecimalConverter))
                .SetConverterFormat("0.00");

            var engine = new FluentFixedEngine(descriptor);

            var itens = engine.ReadString("Product 1 000000000000000129");
        }

        [TestMethod]
        public void Read_With_Decimal_Positive_N_Format()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(10)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Price")
                .SetLength(18)
                .SetConverter(typeof(DecimalConverter))
                .SetConverterFormat("N2");

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString("Product 1 000000000000000129");

            Assert.AreEqual(1, items.Length);
            dynamic item = items[0];
            Assert.AreEqual(1.29M, item.Price);
        }

        [TestMethod]
        public void Read_With_Decimal_Positive_N_3_Format()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(10)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Price")
                .SetLength(18)
                .SetConverter(typeof(DecimalConverter))
                .SetConverterFormat("N3");

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString("Product 1 000000000000000129");

            Assert.AreEqual(1, items.Length);
            dynamic item = items[0];
            Assert.AreEqual(0.129M, item.Price);
        }

        [TestMethod]
        public void Read_With_Decimal_Negative()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(10)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Price")
                .SetLength(18)
                .SetConverter(Type.GetType("FileHelpers.Core.Converters.DecimalConverter, FileHelpers.Fluent", false))
                .SetConverterFormat("0.00");

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString("Product 1 -00000000000000129");

            Assert.AreEqual(1, items.Length);
            dynamic item = items[0];
            Assert.AreEqual(-1.29M, item.Price);
        }

        [TestMethod]
        public void Read_With_Float_Negative()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(10)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Price")
                .SetLength(12)
                .SetConverter(typeof(FloatConverter))
                .SetConverterFormat("0.00");

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString("Product 1 -00000000129");

            Assert.AreEqual(1, items.Length);
            dynamic item = items[0];
            Assert.AreEqual(-1.29F, item.Price);
        }

        [TestMethod]
        public void Read_With_Double_Negative_LessChars()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                      .SetLength(10)
                      .SetTrimMode(TrimMode.Both);

            descriptor.AddField("Price")
                .SetLength(12)
                .SetConverter(typeof(DoubleConverter))
                .SetConverterFormat("0.00");

            var engine = new FluentFixedEngine(descriptor);

            var items = engine.ReadString("Product 1 -129");

            Assert.AreEqual(1, items.Length);
            dynamic item = items[0];
            Assert.AreEqual(1.29, item.Price);
        }

        [TestMethod]
        public void Read_With_Null()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("Name")
                .SetLength(5)
                .SetTrimMode(TrimMode.Both);

            descriptor.AddField("MiddleName")
                .SetLength(5);

            descriptor.AddField("LastName")
                .SetLength(5);

            var engine = descriptor.Build();


            var items = engine.ReadFile(@"C:\tmp\Message.txt");

            Assert.AreEqual(1, items.Length);

            dynamic item = items[0];

            Assert.AreEqual(null, item.MiddleName);
        }

        [TestMethod]
        public void Read_Multi_Array()
        {
            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("MessageAction")
                .SetLength(32)
                .SetTrimMode(TrimMode.Right);

            descriptor.AddField("NUMPARAM")
                .SetLength(9)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            descriptor.AddField("TPCONTR")
                .SetLength(1)
                .SetTrimMode(TrimMode.None)
                ;

            descriptor.AddField("TPPARM")
                .SetLength(1)
                .SetTrimMode(TrimMode.None)
                ;
            descriptor.AddField("TPCHASSI")
                .SetLength(1)
                .SetTrimMode(TrimMode.None)
                ;
            descriptor.AddField("NUMCDB")
                .SetLength(10)
                .SetTrimMode(TrimMode.Both);

            descriptor.AddField("TPBONUS")
                .SetLength(1)
                .SetTrimMode(TrimMode.None)
                ;

            descriptor.AddField("NUMCONTRINI")
                .SetLength(9)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            descriptor.AddField("NUMCONTRFIM")
                .SetLength(9)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            descriptor.AddField("TXTDESCRICAO")
                .SetLength(80)
                .SetTrimMode(TrimMode.Right);

            var accountingEventDescriptor = descriptor.AddArray("AccountingEvent")
                .SetAlign(true)
                .SetAlignChar(' ')
                .SetArrayLength(5590)
                .SetArrayItemLength(1118);

            accountingEventDescriptor.AddField("NUMPARAM")
                .SetLength(9)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            accountingEventDescriptor.AddField("NUMEVENT")
                .SetLength(9)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            accountingEventDescriptor.AddField("TXTDESCRICAO")
                .SetLength(80)
                .SetTrimMode(TrimMode.Right);

            var accountingDataDescriptor = accountingEventDescriptor.AddSubArray("AccountingData")
                .SetAlign(true)
                .SetAlignChar(' ')
                .SetArrayLength(1020)
                .SetArrayItemLength(102);

            accountingDataDescriptor.AddField("NUMPARAM")
                .SetLength(9)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            accountingDataDescriptor.AddField("NUMEVENT")
                .SetLength(9)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            accountingDataDescriptor.AddField("CODTIPO")
                .SetLength(2)
                .SetTrimMode(TrimMode.None);

            accountingDataDescriptor.AddField("NUMSEQ")
                .SetLength(4)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar('0')
                .SetType(typeof(int))
                .SetTrimMode(TrimMode.Both);

            accountingDataDescriptor.AddField("DESCCONTA")
                .SetLength(10)
                .SetTrimMode(TrimMode.Right);

            accountingDataDescriptor.AddField("DESCCENTRO")
                .SetLength(10)
                .SetTrimMode(TrimMode.Right);

            accountingDataDescriptor.AddField("VLRPERCEN")
                .SetLength(5)
                .SetAlignMode(AlignMode.Right)
                .SetAlignChar(' ')
                .SetConverterFormat("N2")
                .SetConverter(typeof(DecimalConverter))
                .SetTrimMode(TrimMode.Both);

            accountingDataDescriptor.AddField("CODLCTO")
                .SetLength(2)
                .SetTrimMode(TrimMode.None);

            accountingDataDescriptor.AddField("CODCOPA")
                .SetLength(1)
                .SetTrimMode(TrimMode.None);

            accountingDataDescriptor.AddField("TXTDESCRICAO")
                .SetLength(50)
                .SetTrimMode(TrimMode.Right);

            var engine = descriptor.Build();

            var items = engine.ReadString("LOAD                            000000001PRC0000000000 000000000000000000SETUP PADRÃO - REGRA DEFAULT                                                    000000001000000001Evento default                                                                  000000001000000001CR0001244044    2PPM      09450  NPARCELA PM - SET UP PADRÃO                        000000001000000001CR0002244048    2PPM      00200  NPM  SET UP PADRÃO - FUNDO DE CONTINGENCIA         000000001000000001CR0003244049    2PPM      00250  NPM SET UP PADRÃO - TX ADMINISTRATIVA              000000001000000001CR0004244050    2PPM      00100  NPM SET UP PADRÃO - FIGHTING FUND                  000000001000000001DB0001PARMA     2PPM      10000  NPARCELA PM SET UP PADRÃO                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ");
        }
    }
}
