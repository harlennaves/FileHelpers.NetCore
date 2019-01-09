using System;
using System.Dynamic;

namespace FileHelpers.Fluent.Events
{
    public class FluentReadEventArgs : FluentEventArgs
    {

        public string Line { get; set; }

        
        public FluentReadEventArgs(string line)
            : this(line, -1)
        {

        }

        public FluentReadEventArgs(string line, int lineNumber)
        {
            Line = line;
            LineNumber = lineNumber;
        }
    }
}
