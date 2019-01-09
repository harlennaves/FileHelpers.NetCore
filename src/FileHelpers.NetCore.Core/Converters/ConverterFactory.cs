using System;
using System.Collections.Concurrent;

namespace FileHelpers.Core.Converters
{
    public static class ConverterFactory
    {
        private static readonly ConcurrentDictionary<string, ConverterBase> converterInstances = new ConcurrentDictionary<string, ConverterBase>();

        public static ConverterBase GetConverter(Type type, string format)
        {
            string instanceName = type.FullName;
            if (!string.IsNullOrWhiteSpace(format))
                instanceName += $"_{format}";

            if (converterInstances.TryGetValue(instanceName, out ConverterBase converterInstance))
                return converterInstance;

            converterInstance = (string.IsNullOrWhiteSpace(format)
                ? Activator.CreateInstance(type)
                : Activator.CreateInstance(type, format)) as ConverterBase;

            converterInstances.TryAdd(instanceName, converterInstance);
            return converterInstance;
        }
    }
}
