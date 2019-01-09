using System;

namespace FileHelpers.Fluent.Events
{
    public class FluentEventArgs : EventArgs
    {
        public bool SkipRecord { get; set; }

        public bool LineChanged { get; set; }

        public int LineNumber { get; set; }
    }
}
