using System.Dynamic;

namespace FileHelpers.NetCore.Fluent.Events
{
    public class BeforeFluentWriteEventArgs : FluentWriteEventArgs
    {

        public BeforeFluentWriteEventArgs(ExpandoObject record, int recordLine)
        {
            LineNumber = recordLine;
            Record = record;
        }
    }
}
