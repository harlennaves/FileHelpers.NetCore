using System.Collections.Generic;
using System.Dynamic;
using System.Text;

using FileHelpers.NetCore.Builders.Core;
using FileHelpers.NetCore.Fluent.Builders;

namespace FileHelpers.NetCore.Fluent.Extensions
{
    public static class FluentFixedArrayFieldBuilderExtensions
    {
        public static string ArrayToString(this IFluentFixedArrayRecordInfo recordInfo, IEnumerable<dynamic> array)
        {
            var sb = new StringBuilder(recordInfo.ArrayLength);

            var fieldRecordInfo = recordInfo as IFluentFixedBuilder;

            if (fieldRecordInfo == null)
            {
                if (recordInfo.Align)
                    sb.Append(recordInfo.AlignChar, recordInfo.ArrayLength);
                return sb.ToString();
            }

            foreach (ExpandoObject item in array)
            {
                var record = item as IDictionary<string, object>;
                foreach (KeyValuePair<string, IFluentFixedRecordInfoBase> fluentFixedRecordInfoBase in fieldRecordInfo.Fields)
                {
                    if (fluentFixedRecordInfoBase.Value.IsArray)
                    {
                        sb.Append(((IFluentFixedArrayRecordInfo)fluentFixedRecordInfoBase.Value).ArrayToString(
                            (dynamic[])record[fluentFixedRecordInfoBase.Key]));
                        continue;
                    }

                    sb.Append(((IFluentFixedRecordInfo)fluentFixedRecordInfoBase.Value).RecordToString(
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
    }
}
