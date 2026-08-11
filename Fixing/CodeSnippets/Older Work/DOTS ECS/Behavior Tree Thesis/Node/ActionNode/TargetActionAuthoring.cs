using UnityEngine;
using Unity.Entities;

public class TargetActionAuthoring : ActionNodeAuthoring {
    
    public GameObject target;

    public class TargetActionAuthoringBaker : Baker<TargetActionAuthoring> {
        public override void Bake(TargetActionAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Target {
                target = authoring.target != null ? GetEntity(authoring.target, TransformUsageFlags.Dynamic) : Entity.Null
            });
        }
    }

}

public struct Target : IComponentData {
    public float speed;
    public Entity target;
}
