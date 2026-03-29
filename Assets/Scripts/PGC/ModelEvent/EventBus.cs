using System;
using System.Collections.Generic;

namespace PGC.ModelEvent
{
    public class EventBus
    {
        private Dictionary<Type, List<Delegate>> handles = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!handles.ContainsKey(type))
            {
                handles[type] = new List<Delegate>();
            }
            handles[type].Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            if (handles.TryGetValue(typeof(T), out var list))
            {
                list.Remove(handler);
            }
        }

        public void Publish<T>(T message)
        {
            var type = typeof(T);
            if (handles.TryGetValue(type, out var list))
            {
                foreach (var handle in list)
                {
                    ((Action<T>)handle)?.Invoke(message);
                }
            }
        }
        
    }
}