using System.Dynamic;

namespace FileHelpers.NetCore.Fluent.Events
{
    public class FluentWriteEventArgs : FluentEventArgs
    {
        public ExpandoObject Record { get; protected set; }
    }
}
