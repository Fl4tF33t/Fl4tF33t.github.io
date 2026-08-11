using UnityEngine;

public struct Empty {
}
[CreateAssetMenu(menuName = "Events/Empty Event Channel")]
public class EmptyChannel : EventChannel<Empty> {
}