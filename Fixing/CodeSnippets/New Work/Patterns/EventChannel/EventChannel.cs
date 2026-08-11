using UnityEngine;
using UnityEngine.Events;

// can use editor property drawer to have the Event be raised from the inspector
// Custom asset menu will not work with a Generic type, make non-generic to use that feature
public abstract class EventChannel<T> : ScriptableObject {
    public UnityAction<T> OnEventRaised;

    public void RaiseEvent(T value) {
        OnEventRaised?.Invoke(value);
    }
}