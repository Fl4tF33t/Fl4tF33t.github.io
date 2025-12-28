using Unity.Entities;
using UnityEngine;

public class WaitActionAuthoring : ActionNodeAuthoring {
    public float timer;

    public class WaitActionAuthoringBaker : Baker<WaitActionAuthoring> {
        public override void Bake(WaitActionAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Timer {
                Value = authoring.timer,
                timer = authoring.timer
            });
        }
    }

}

public struct Timer : IComponentData {
    public float timer;
    public float Value;
}
