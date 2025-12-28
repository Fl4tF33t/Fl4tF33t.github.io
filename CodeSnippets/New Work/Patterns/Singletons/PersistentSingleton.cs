using UnityEngine;

namespace Patterns {

    public abstract class PersistentSingleton<T> : MonoBehaviour where T : PersistentSingleton<T> {
        private static T instance;

        public static T Instance {
            get {
                if (instance == null) {
#if UNITY_2023_1_OR_NEWER
                    instance = FindFirstObjectByType<T>();
#else
                    instance = FindObjectOfType<T>();
#endif

                    if (instance == null) {
                        var go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake() {
            if (instance == null) {
                instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this) Destroy(gameObject);
        }

        protected virtual void OnDestroy() {
            if (instance == this) instance = null;
        }
    }
}