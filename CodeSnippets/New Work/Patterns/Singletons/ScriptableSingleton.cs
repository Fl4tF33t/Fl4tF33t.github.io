using UnityEngine;

namespace Patterns {
    public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T> {
        private static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    instance = Resources.Load<T>(typeof(T).Name);
                    
                    if (instance == null) instance = CreateInstance<T>();
                }

                return instance;
            }
        }
    }
}