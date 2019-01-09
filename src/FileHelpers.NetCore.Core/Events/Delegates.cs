using FileHelpers.Core.Engines;

namespace FileHelpers.Fluent.Events
{
    public delegate void BeforeFluentReadHandler(IFluentEngine engine, FluentReadEventArgs args);

    public delegate void AfterFluentReadHandler(IFluentEngine engine, AfterFluentReadEventArgs args);

    public delegate void BeforeFluentWriteHandler(IFluentEngine engine, BeforeFluentWriteEventArgs args);

    public delegate void AfterFluentWriteHandler(IFluentEngine engine, AfterFluentWriteEventArgs args);
}
