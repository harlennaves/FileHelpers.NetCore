using System.Dynamic;

namespace FileHelpers.Fluent.Events
{
    public class FluentWriteEventArgs : FluentEventArgs
    {
        public ExpandoObject Record { get; protected set; }
    }
}
