using System;
using UnityEngine;

namespace Extensions {
    public static class GameObjectExtensions {
        public static T GetOrAdd<T>(this GameObject go) where T : Component {
            if (go == null) throw new ArgumentNullException(nameof(go));

            T component = go.GetComponent<T>();
            if (component == null) {
                component = go.AddComponent<T>();
            }
            return component;
        }

        public static T GetRequired<T>(this GameObject go) where T : Component {
            var component = go.GetComponent<T>();
            if (component == null)
                throw new MissingComponentException(
                    $"{typeof(T)} is required on {go.name}");
            return component;
        }

        public static bool Has<T>(this GameObject go) where T : Component {
            return go.GetComponent<T>() != null;
        }

        public static void SetActiveRecursively(this GameObject go, bool isActive) {
            if (go == null) throw new ArgumentNullException(nameof(go));
            go.SetActive(isActive);
            foreach (Transform child in go.transform) {
                child.gameObject.SetActiveRecursively(isActive);
            }
        }

        public static void SetActiveSafe(this GameObject go, bool value) {
            if (go != null && go.activeSelf != value)
                go.SetActive(value);
        }

        public static void DestroyChildren(this GameObject go) {
            foreach (Transform child in go.transform) {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

    }
}
