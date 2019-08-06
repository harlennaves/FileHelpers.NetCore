using FileHelpers.Core.Converters;
using FileHelpers.Fluent.Delimited.Descriptors;
using FileHelpers.Fluent.Delimited.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace FileHelpers.Fluent.Delimited.UnitTests
{
    [TestClass]
    public class DelimitedEngineUnitTest
    {
        [TestMethod]
        public void Create_BaseEngine()
        {

            var descriptor = new DelimitedRecordDescriptor("|");

            descriptor.AddField("Name")
                .SetTrimMode(TrimMode.Both)
                .SetAlignMode(AlignMode.Left)
                ;

            descriptor.AddField("BirthDate")
                .SetType(typeof(DateTime))
                .SetConverterFormat("yyyyMMdd")
                ;

            var engine = new DelimitedFluentEngine(descriptor);
            var items = engine.ReadString("HARLEN      |19840330");
        }
        [TestMethod]
        public void Create_Array()
        {
            var descriptor = new DelimitedRecordDescriptor("|");

            descriptor.AddField("Name")
                .SetTrimMode(TrimMode.Both)
                .SetAlignMode(AlignMode.Left)
                ;

            descriptor.AddField("BirthDate")
                .SetConverter(typeof(DateTimeConverter))
                .SetConverterFormat("yyyyMMdd")
                ;

            var arrayDescriptor = descriptor.AddArray("Items")
                .SetArrayDelimiter("#")
                .SetArrayItemBegin("{")
                .SetArrayItemEnd("}");

            arrayDescriptor.AddField("ItemName")
                .SetTrimMode(TrimMode.Both);

            arrayDescriptor.AddField("ItemPrice")
                .SetType(typeof(decimal))
                .SetTrimMode(TrimMode.Both);

            var engine = new DelimitedFluentEngine(descriptor);

            var items = engine.ReadString("Harlen|19840330|{Item1#2.00}{Item2#3.59}|Renato Polatti");
        }

        [TestMethod]
        public void Write_Single_Record()
        {
            var descriptor = new DelimitedRecordDescriptor("|");

            descriptor.AddField("Name")
                .SetTrimMode(TrimMode.Both)
                .SetAlignMode(AlignMode.Left)
                ;

            descriptor.AddField("BirthDate")
                .SetConverter(typeof(DateTimeConverter))
                .SetConverterFormat("yyyyMMdd")
                ;
            var engine = new DelimitedFluentEngine(descriptor);

            var item = new ExpandoObject();
            item.TryAdd("Name", "Harlen Naves");
            item.TryAdd("BirthDate", new DateTime(1984, 03, 30));
            var content = engine.WriteString(new [] { item });
            
        }

        [TestMethod]
        public void Write_Array_Record()
        {
            var descriptor = new DelimitedRecordDescriptor("|");

            descriptor.AddField("Name")
                .SetTrimMode(TrimMode.Both)
                .SetAlignMode(AlignMode.Left)
                ;

            descriptor.AddField("BirthDate")
                .SetConverter(typeof(DateTimeConverter))
                .SetConverterFormat("yyyyMMdd")
                ;

            var arrayDescriptor = descriptor.AddArray("Items")
                .SetArrayDelimiter("#")
                .SetArrayItemBegin("{")
                .SetArrayItemEnd("}");

            arrayDescriptor.AddField("ItemName")
                .SetTrimMode(TrimMode.Both);

            arrayDescriptor.AddField("ItemPrice")
                .SetType(typeof(decimal))
                .SetTrimMode(TrimMode.Both);

            var engine = new DelimitedFluentEngine(descriptor);
            var content = "Harlen|19840330|{Item1#2.00}{Item2#3.59}|Renato Polatti";
            var items = engine.ReadString(content);

            var writenContent = engine.WriteString(items);

            Assert.AreEqual(content, writenContent);
        }
    }
}
