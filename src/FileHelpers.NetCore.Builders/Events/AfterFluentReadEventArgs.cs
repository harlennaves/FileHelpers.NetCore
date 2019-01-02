using System.Dynamic;

namespace FileHelpers.NetCore.Fluent.Events
{
    public class AfterFluentReadEventArgs : FluentReadEventArgs
    {
        public ExpandoObject Record { get; set; }

        public AfterFluentReadEventArgs(string line) : base(line)
        {
        }

        public AfterFluentReadEventArgs(string line, int lineNumber) : base(line, lineNumber)
        {
        }
    }
}
