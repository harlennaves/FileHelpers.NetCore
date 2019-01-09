namespace FileHelpers.Fluent.Events
{
    public class BeforeFluentReadEventArgs : FluentReadEventArgs
    {
        public BeforeFluentReadEventArgs(
            string line
        ) : base(line)
        {
            
        }

        public BeforeFluentReadEventArgs(string line, int lineNumber) : base(line, lineNumber)
        { }
    }
}
