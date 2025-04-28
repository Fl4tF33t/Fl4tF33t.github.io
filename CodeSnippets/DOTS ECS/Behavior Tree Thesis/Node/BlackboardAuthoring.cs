using UnityEngine;
using Unity.Entities;
public class BlackboardAuthoring : MonoBehaviour {
    public GameObject target;

    public class BlackboardBaker : Baker<BlackboardAuthoring> {
        public override void Bake(BlackboardAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Blackboard>(entity);
        }
    }
}

public struct Blackboard : IComponentData {
    public Entity Value;
}
