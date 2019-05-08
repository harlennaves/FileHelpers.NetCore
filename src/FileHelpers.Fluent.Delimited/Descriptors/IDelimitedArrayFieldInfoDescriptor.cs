using FileHelpers.Core.Descriptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileHelpers.Fluent.Delimited.Descriptors
{
    public interface IDelimitedArrayFieldInfoDescriptor : IFieldInfoTypeDescriptor, IRecordDescriptor
    {
        string ArrayDelimiter { get; set; }

        string ArrayItemBegin { get; set; }

        string ArrayItemEnd { get; set; }
    }
}
