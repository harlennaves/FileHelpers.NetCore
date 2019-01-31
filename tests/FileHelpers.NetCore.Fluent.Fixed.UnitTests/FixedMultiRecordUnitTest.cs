using System;
using System.Collections.Generic;
using System.Dynamic;

using FileHelpers.Core.Converters;
using FileHelpers.Fluent;
using FileHelpers.Fluent.Fixed;
using FileHelpers.Fluent.Fixed.Core;
using FileHelpers.Fluent.Fixed.Descriptors;
using FileHelpers.Fluent.Fixed.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileHelpers.NetCore.Fluent.Fixed.UnitTests
{
    [TestClass]
    public class FixedMultiRecordUnitTest
    {
        [TestMethod]
        public void MultiRecord_Read()
        {
            var clientDescriptor = new FixedRecordDescriptor();

            clientDescriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Name")
                            .SetLength(50)
                            .SetAlignMode(AlignMode.Right)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Doc")
                      .SetLength(14)
                      .SetAlignMode(AlignMode.Left)
                      .SetConverter(typeof(LongConverter))
                      .SetAlignChar('0');

            clientDescriptor.AddField("BirthDate")
                      .SetLength(8)
                      .SetConverter(typeof(DateTimeConverter))
                      .SetConverterFormat("yyyyMMdd");

            var addressDescriptor = new FixedRecordDescriptor();

            addressDescriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

            addressDescriptor.AddField("Street")
                             .SetLength(20)
                             .SetTrimMode(TrimMode.Both)
                             .SetAlignMode(AlignMode.Right)
                             .SetAlignChar(' ');
            addressDescriptor.AddField("City")
                             .SetLength(20)
                             .SetTrimMode(TrimMode.Both)
                             .SetAlignMode(AlignMode.Right)
                             .SetAlignChar(' ');


            var engine = new FluentFixedMultiRecordEngine("RecordType",
                new MultiRecordItem
                {
                    Descriptor = clientDescriptor,
                    Name = "Client",
                    RegexPattern = "^(CLI)"
                },
                new MultiRecordItem
                {
                    Descriptor = addressDescriptor,
                    Name = "Address",
                    RegexPattern = "^(ADR)"
                }
            );

            string content =
                "CLIHarlen Naves                                      0000587065966319840330\r\nADRNo name street      No name city        ";

            var items = engine.ReadString(content);
            
        }
        [TestMethod]
        public void Write()
        {
            var clientDescriptor = new FixedRecordDescriptor();

            clientDescriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Name")
                            .SetLength(50)
                            .SetAlignMode(AlignMode.Right)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Doc")
                      .SetLength(14)
                      .SetAlignMode(AlignMode.Left)
                      .SetConverter(typeof(LongConverter))
                      .SetAlignChar('0');

            clientDescriptor.AddField("BirthDate")
                      .SetLength(8)
                      .SetConverter(typeof(DateTimeConverter))
                      .SetConverterFormat("yyyyMMdd");

            var addressDescriptor = new FixedRecordDescriptor();

            addressDescriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

            addressDescriptor.AddField("Street")
                             .SetLength(20)
                             .SetTrimMode(TrimMode.Both)
                             .SetAlignMode(AlignMode.Right)
                             .SetAlignChar(' ');
            addressDescriptor.AddField("City")
                             .SetLength(20)
                             .SetTrimMode(TrimMode.Both)
                             .SetAlignMode(AlignMode.Right)
                             .SetAlignChar(' ');


            var engine = new FluentFixedMultiRecordEngine("RecordType",
                new MultiRecordItem
                {
                    Descriptor = clientDescriptor,
                    Name = "Client",
                    RegexPattern = "^(CLI)"
                },
                new MultiRecordItem
                {
                    Descriptor = addressDescriptor,
                    Name = "Address",
                    RegexPattern = "^(ADR)"
                }
            );
            string content =
                "CLIHarlen Naves                                      0000587065966319840330\r\nADRNo name street      No name city        ";

            var items = engine.ReadString(content);

            string output = engine.WriteString(items);

            Assert.AreEqual(content + Environment.NewLine, output);
        }

        [TestMethod]
        public void Read_With_Array()
        {
            var clientDescriptor = new FixedRecordDescriptor();

            clientDescriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Name")
                            .SetLength(50)
                            .SetAlignMode(AlignMode.Right)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Doc")
                            .SetLength(14)
                            .SetAlignMode(AlignMode.Left)
                            .SetConverter(typeof(LongConverter))
                            .SetAlignChar('0');

            clientDescriptor.AddField("BirthDate")
                            .SetLength(8)
                            .SetConverter(typeof(DateTimeConverter))
                            .SetConverterFormat("yyyyMMdd");

            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

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

            var engine = new FluentFixedMultiRecordEngine("RecordType",
                new MultiRecordItem
                {
                    Descriptor = clientDescriptor,
                    Name = "Client",
                    RegexPattern = "^(CLI)"
                },
                new MultiRecordItem
                {
                    Descriptor = descriptor,
                    Name = "Test",
                    RegexPattern = "^(TST)"
                });

            string content =
                "TST0010025STOCKASC00STOCKASC01STOCKASC02STOCKASC03STOCKASC04STOCKASC05STOCKASC06STOCKASC07STOCKASC08STOCKASC09STOCKASC10STOCKASC11STOCKASC12STOCKASC13STOCKASC14STOCKASC15STOCKASC16STOCKASC17STOCKASC18STOCKASC19STOCKASC20STOCKASC21STOCKASC22STOCKASC23STOCKASC240000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                Environment.NewLine + "CLIHarlen Naves                                      0000587065966319840330" + Environment.NewLine;

            var records = engine.ReadString(content);
        }

        [TestMethod]
        public void Write_With_Array()
        {
            var clientDescriptor = new FixedRecordDescriptor();

            clientDescriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Name")
                            .SetLength(50)
                            .SetAlignMode(AlignMode.Right)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignChar(' ');

            clientDescriptor.AddField("Doc")
                            .SetLength(14)
                            .SetAlignMode(AlignMode.Left)
                            .SetConverter(typeof(LongConverter))
                            .SetAlignChar('0');

            clientDescriptor.AddField("BirthDate")
                            .SetLength(8)
                            .SetConverter(typeof(DateTimeConverter))
                            .SetConverterFormat("yyyyMMdd");

            var descriptor = new FixedRecordDescriptor();

            descriptor.AddField("RecordType")
                            .SetLength(3)
                            .SetTrimMode(TrimMode.Both)
                            .SetAlignMode(AlignMode.Right)
                            .SetAlignChar(' ');

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

            var engine = new FluentFixedMultiRecordEngine("RecordType",
                new MultiRecordItem
                {
                    Descriptor = clientDescriptor,
                    Name = "Client",
                    RegexPattern = "^(CLI)"
                },
                new MultiRecordItem
                {
                    Descriptor = descriptor,
                    Name = "Test",
                    RegexPattern = "^(TST)"
                });

            string content =
                "TST0010025STOCKASC00STOCKASC01STOCKASC02STOCKASC03STOCKASC04STOCKASC05STOCKASC06STOCKASC07STOCKASC08STOCKASC09STOCKASC10STOCKASC11STOCKASC12STOCKASC13STOCKASC14STOCKASC15STOCKASC16STOCKASC17STOCKASC18STOCKASC19STOCKASC20STOCKASC21STOCKASC22STOCKASC23STOCKASC240000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" +
                Environment.NewLine + "CLIHarlen Naves                                      0000587065966319840330" + Environment.NewLine;

            var records = engine.ReadString(content);

            var output = engine.WriteString(records);

            Assert.AreEqual(content, output);
        }
    }
}
