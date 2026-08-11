using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Patterns;
using Object = UnityEngine.Object;

namespace Utils {
    [Serializable]
    public class ObjectPoolConfig {
        public bool collectionCheck = true;
        public int defaultCapacity = 10;
        public int maxCapacity = 30;
    }

// NOTE: One pool per component type.
// Multiple prefabs per type are not supported.
    public class ObjectPoolManager : SceneSingleton<ObjectPoolManager> {
        [SerializeField] private ObjectPoolConfig poolConfig = new();
        private readonly Dictionary<Type, object> pools = new();

        public T Spawn<T>(ObjectPoolConfig config = null) where T : Component => GetOrCreatePool<T>(config).Get();

        public void Despawn<T>(T arg) where T : Component => GetOrCreatePool<T>().Release(arg);

        private IObjectPool<T> GetOrCreatePool<T>(ObjectPoolConfig config = null) where T : Component {
            var type = typeof(T);

            if (pools.TryGetValue(type, out var pool))
                return (IObjectPool<T>)pool;

            var newPool = CreatePool<T>(config);
            pools[type] = newPool;
            return newPool;
        }

        private IObjectPool<T> CreatePool<T>(ObjectPoolConfig config = null) where T : Component {
            if (!ServiceLocator.TryGet(out IFactory<T> factory))
                throw new System.InvalidOperationException($"No factory registered for type {typeof(T)}");

            config ??= poolConfig;

            return new ObjectPool<T>(
                createFunc: factory.Create,
                actionOnGet: obj => obj.gameObject.SetActive(true),
                actionOnRelease: obj => obj.gameObject.SetActive(false),
                actionOnDestroy: obj => Object.Destroy(obj.gameObject),
                collectionCheck: config.collectionCheck,
                defaultCapacity: config.defaultCapacity,
                maxSize: config.maxCapacity
            );
        }

        protected override void OnDestroy() {
            pools.Clear();
            base.OnDestroy();
        }
    }
}