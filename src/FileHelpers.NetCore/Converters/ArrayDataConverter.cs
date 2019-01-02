using System;
using System.Collections.Generic;
using System.Reflection;

using FileHelpers.Events;

namespace FileHelpers
{
    public class ArrayDataConverter<TEntity> : ConverterBase
        where TEntity : class
    {
        private static IDictionary<Type, FileHelperEngine<TEntity>> engines = new Dictionary<Type, FileHelperEngine<TEntity>>();

        public override object StringToField(string from) => from;

        public override string FieldToString(object from) =>
            Serialize(from == null ? Activator.CreateInstance<TEntity>() : (TEntity)from);

        private string Serialize(TEntity from)
        {
            var engine = GetEngine();

            return engine.WriteString(new List<TEntity>() { from }).TrimEnd('\n', '\r');
        }

        private FileHelperEngine<TEntity> GetEngine()
        {
            if (engines.TryGetValue(typeof(TEntity), out FileHelperEngine<TEntity> engine))
                return engine;

            engine = new FileHelperEngine<TEntity>();

            Type entityType = typeof(TEntity);

            MethodInfo beforeEvent = entityType.GetMethod("BeforeEvent");
            if (beforeEvent != null)
            {
                engine.BeforeWriteRecord += (BeforeWriteHandler<TEntity>)Delegate.CreateDelegate(typeof(BeforeWriteHandler<TEntity>), beforeEvent);
            }

            var afterEvent = entityType.GetMethod("AfterEvent");

            if (afterEvent != null)
                engine.AfterWriteRecord +=
                    (AfterWriteHandler<TEntity>)Delegate.CreateDelegate(typeof(AfterWriteHandler<TEntity>), afterEvent);

            engines.Add(entityType, engine);

            return engine;
        }
    }
}
