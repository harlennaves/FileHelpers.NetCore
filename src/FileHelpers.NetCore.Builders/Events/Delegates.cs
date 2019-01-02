using FileHelpers.NetCore.Fluent.Engines;

namespace FileHelpers.NetCore.Fluent.Events
{
    public delegate void BeforeFluentReadHandler(FluentEngineBase engine, FluentReadEventArgs args);

    public delegate void AfterFluentReadHandler(FluentEngineBase engine, AfterFluentReadEventArgs args);

    public delegate void BeforeFluentWriteHandler(FluentEngineBase engine, BeforeFluentWriteEventArgs args);

    public delegate void AfterFluentWriteHandler(FluentEngineBase engine, AfterFluentWriteEventArgs args);
}
