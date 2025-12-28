using System;
using UnityEngine;

namespace Extensions {
    public static class MonoBehaviourExtensions {
        public static void InvokeNextFrame(this MonoBehaviour mb, Action action) {
            mb.StartCoroutine(InvokeNextFrameRoutine(action));
        }

        private static IEnumerator InvokeNextFrameRoutine(Action action) {
            yield return null;
            action?.Invoke();
        }

        public static void StopAndClear(this MonoBehaviour mb, ref Coroutine coroutine) {
            if (coroutine == null) return;
            mb.StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}