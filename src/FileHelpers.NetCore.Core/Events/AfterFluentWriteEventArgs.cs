using System.Dynamic;

namespace FileHelpers.Fluent.Events
{
    public class AfterFluentWriteEventArgs : FluentWriteEventArgs
    {
        public string Line { get; }

        public bool LineChanged { get; set; }

        public AfterFluentWriteEventArgs(ExpandoObject record, int lineNumber, string line)
        {
            Record = record;
            LineNumber = lineNumber;
            Line = line;
        }
    }
}
