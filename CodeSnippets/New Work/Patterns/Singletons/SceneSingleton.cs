using UnityEngine;

namespace Patterns {
    public abstract class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T> {
        private static T instance;

        public static T Instance {
            get {
                if (instance == null) {
#if UNITY_2023_1_OR_NEWER
                    instance = FindFirstObjectByType<T>();
#else
                    instance = FindObjectOfType<T>();
#endif

                    if (instance == null) Debug.LogError($"No {typeof(T).Name} found in the active scene.");
                }
                return instance;
            }
        }

        protected virtual void Awake() {
            if (instance == null) instance = (T)this;
            else if (instance != this) Destroy(gameObject);
        }

        protected virtual void OnDestroy() {
            if (instance == this) instance = null;
        }
    }
}