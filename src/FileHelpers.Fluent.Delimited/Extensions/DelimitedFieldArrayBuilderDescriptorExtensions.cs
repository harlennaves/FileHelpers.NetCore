using FileHelpers.Core.Descriptors;
using FileHelpers.Fluent.Delimited.Descriptors;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace FileHelpers.Fluent.Delimited.Extensions
{
    internal static class DelimitedFieldArrayBuilderDescriptorExtensions
    {
        internal static string ArrayToString(this IDelimitedArrayFieldInfoDescriptor recordInfo, IEnumerable<dynamic> array)
        {
            var sb = new StringBuilder();

            var fieldRecordInfo = recordInfo as IRecordDescriptor;

            foreach (ExpandoObject item in array)
            {
                var record = item as IDictionary<string, object>;
                foreach (var delimitedRecordInfoBase in fieldRecordInfo.Fields)
                {

                }
            }

            return sb.ToString();
        }

        internal static dynamic[] StringToArray(this IDelimitedArrayFieldInfoDescriptor recordInfo, string line, char nullChar)
        {
            var fieldRecordInfo = recordInfo as IRecordDescriptor;
            
            if (fieldRecordInfo == null)
                return new dynamic[0];

            IList<dynamic> items = new List<dynamic>();

            string[] arrayItems = line.Split(recordInfo.ArrayItemEnd, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 0; index < arrayItems.Length; index++)
            {
                var item = new ExpandoObject();
                int fieldIndex = 0;
                string [] fieldsValue = arrayItems[index].Replace(recordInfo.ArrayItemBegin, string.Empty)
                    .Split(recordInfo.ArrayDelimiter);
                foreach (var fluentFixedRecordInfoBase in fieldRecordInfo.Fields)
                {
                    string fieldValue = fieldIndex >= fieldsValue.Length ? null : fieldsValue[fieldIndex];
                    item.TryAdd(
                            fluentFixedRecordInfoBase.Key,
                            fluentFixedRecordInfoBase.Value.IsArray
                                ? ((IDelimitedArrayFieldInfoDescriptor)fluentFixedRecordInfoBase.Value)
                                    .StringToArray(fieldValue, nullChar)
                                : ((IFieldInfoDescriptor)fluentFixedRecordInfoBase.Value)
                                    .StringToRecord(fieldValue, nullChar)
                        );
                    fieldIndex++;
                }
                items.Add(item);
                
            }

            return items.ToArray();
        }
    }
}
