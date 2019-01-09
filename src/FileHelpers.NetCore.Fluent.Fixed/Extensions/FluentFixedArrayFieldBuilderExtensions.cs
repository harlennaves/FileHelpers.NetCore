using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Fixed.Descriptors;

namespace FileHelpers.Fluent.Fixed.Extensions
{
    public static class FluentFixedArrayFieldBuilderExtensions
    {
        public static string ArrayToString(this IArrayFieldInfoDescriptor recordInfo, IEnumerable<dynamic> array)
        {
            var sb = new StringBuilder(recordInfo.ArrayLength);

            var fieldRecordInfo = recordInfo as IRecordDescriptor;

            foreach (ExpandoObject item in array)
            {
                var record = item as IDictionary<string, object>;
                foreach (KeyValuePair<string, IFieldInfoTypeDescriptor> fluentFixedRecordInfoBase in fieldRecordInfo.Fields)
                {
                    if (fluentFixedRecordInfoBase.Value.IsArray)
                    {
                        sb.Append(((IArrayFieldInfoDescriptor)fluentFixedRecordInfoBase.Value).ArrayToString(
                            (dynamic[])record[fluentFixedRecordInfoBase.Key]));
                        continue;
                    }

                    sb.Append(((IFixedFieldInfoDescriptor)fluentFixedRecordInfoBase.Value).RecordToString(
                        record[fluentFixedRecordInfoBase.Key]));
                }
            }

            if (recordInfo.Align)
            {
                int residual = recordInfo.ArrayLength - sb.Length;
                if (residual > 0)
                    sb.Append(recordInfo.AlignChar, residual);
            }

            return sb.ToString();
        }

        public static dynamic[] StringToArray(this IArrayFieldInfoDescriptor recordInfo, string line, ref int offset)
        {
            var fieldRecordInfo = recordInfo as IRecordDescriptor;

            if (fieldRecordInfo == null)
                return new dynamic[0];

            IList<dynamic> items = new List<dynamic>();
            string arrayString = line.Substring(offset, recordInfo.ArrayLength);
            offset += recordInfo.ArrayLength;
            int arrayOffset = 0;

            string arrayItemString = arrayString.Substring(arrayOffset, recordInfo.ArrayItemLength);

            string alignItem = new StringBuilder(recordInfo.ArrayItemLength)
                               .Append(recordInfo.AlignChar, recordInfo.ArrayItemLength).ToString();

            while (arrayItemString != null)
            {
                if (!string.IsNullOrWhiteSpace(arrayItemString) && arrayItemString != alignItem)
                {
                    var item = new ExpandoObject();
                    int itemOffset = 0;
                    foreach (KeyValuePair<string, IFieldInfoTypeDescriptor> fluentFixedRecordInfoBase in fieldRecordInfo.Fields)
                    {
                        if (fluentFixedRecordInfoBase.Value.IsArray)
                        {
                            item.TryAdd(fluentFixedRecordInfoBase.Key,
                                ((IArrayFieldInfoDescriptor)fluentFixedRecordInfoBase.Value).StringToArray(arrayItemString,
                                    ref offset));
                            continue;
                        }

                        item.TryAdd(fluentFixedRecordInfoBase.Key,
                            ((IFixedFieldInfoDescriptor)fluentFixedRecordInfoBase.Value).StringToRecord(arrayItemString,
                                ref itemOffset));
                    }

                    items.Add(item);
                }

                arrayOffset += recordInfo.ArrayItemLength;
                arrayItemString = arrayString.Length < arrayOffset + recordInfo.ArrayItemLength
                    ? null
                    : arrayString.Substring(arrayOffset, recordInfo.ArrayItemLength);
            }

            return items.ToArray();
        }
    }
}
