using System;
using System.Collections.Generic;

namespace Patterns {
    public static class ServiceLocator {
        public static event Action<Type, object> OnServiceRegistered;
        public static event Action<Type, object> OnServiceUnregistered;
        private static readonly Dictionary<Type, object> services = new();

        public static void Register<T>(T service, bool isOverride = false) where T : class {
            if (service == null) throw new ArgumentNullException(nameof(service));

            var type = typeof(T);
            if (!services.TryAdd(type, service) && !isOverride) throw new InvalidOperationException($"Service of type {type.FullName} is already registered");
            
            if (isOverride) services[type] = service;
            
            OnServiceRegistered?.Invoke(type, service);
        }

        public static void Unregister<T>() where T : class {
            var type = typeof(T);
            if (!services.Remove(type, out var removed))
                throw new InvalidOperationException($"Service of type {type.FullName} was not registered");

            OnServiceUnregistered?.Invoke(type, removed);
        }

        public static T Get<T>() where T : class {
            var type = typeof(T);
            if (services.TryGetValue(type, out var result))
                return (T)result;

            throw new InvalidOperationException($"Service of type {type.FullName} is not registered");
        }

        public static bool TryGet<T>(out T service) where T : class {
            if (services.TryGetValue(typeof(T), out var result)) {
                service = (T)result;
                return true;
            }

            service = null;
            return false;
        }
    }
}