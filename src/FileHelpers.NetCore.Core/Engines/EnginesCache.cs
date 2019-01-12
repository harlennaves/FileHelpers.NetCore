using System;
using System.Collections.Concurrent;

namespace FileHelpers.Core.Engines
{
    public static class EnginesCache
    {
        private static readonly ConcurrentDictionary<string, FluentEngineBase> engines = new ConcurrentDictionary<string, FluentEngineBase>();

        public static bool TryGet(string engineKey, out FluentEngineBase engine) => 
            engines.TryGetValue(engineKey, out engine);

        public static bool TryGetOrAdd(string engineKey, out FluentEngineBase engine, Func<FluentEngineBase> addAction)
        {
            if (engines.TryGetValue(engineKey, out engine))
                return true;
            
            return engines.TryAdd(engineKey, addAction()) && TryGet(engineKey, out engine);
        }
    }
}
