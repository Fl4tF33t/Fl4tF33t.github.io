using System;
using UnityEngine;

namespace Extensions {
    public static class TransformExtensions {
        public static void ResetLocalTransform(this Transform transform) {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void SetLayerRecursively(this Transform transform, int layer) {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            transform.gameObject.layer = layer;
            foreach (Transform child in transform) {
                child.SetLayerRecursively(layer);
            }
        }

        public static void DestroyChildren(this Transform transform) {
            if (transform == null) throw new ArgumentNullException(nameof(transform));
            foreach (Transform child in transform) {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        public static void SetX(this Transform t, float x) {
            var pos = t.position;
            pos.x = x;
            t.position = pos;
        }
        public static void SetY(this Transform t, float y) {
            var pos = t.position;
            pos.y = y;
            t.position = pos;
        }
        public static void SetZ(this Transform t, float z) {
            var pos = t.position;
            pos.z = z;
            t.position = pos;
        }
    }
}