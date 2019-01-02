using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

using FileHelpers.NetCore.Builders.Core;
using FileHelpers.NetCore.Fluent.Builders;

namespace FileHelpers.NetCore.Fluent.Extensions
{
    public static class FluentFixedArrayFieldBuilderReadExtensions
    {
        public static dynamic[] StringToArray(this IFluentFixedArrayRecordInfo recordInfo, string line, ref int offset)
        {
            var fieldRecordInfo = recordInfo as IFluentFixedBuilder;

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
                    foreach (KeyValuePair<string, IFluentFixedRecordInfoBase> fluentFixedRecordInfoBase in fieldRecordInfo.Fields)
                    {
                        if (fluentFixedRecordInfoBase.Value.IsArray)
                        {
                            item.TryAdd(fluentFixedRecordInfoBase.Key,
                                ((IFluentFixedArrayRecordInfo)fluentFixedRecordInfoBase.Value).StringToArray(arrayItemString,
                                    ref offset));
                            continue;
                        }

                        item.TryAdd(fluentFixedRecordInfoBase.Key,
                            ((IFluentFixedRecordInfo)fluentFixedRecordInfoBase.Value).StringToRecord(arrayItemString,
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
