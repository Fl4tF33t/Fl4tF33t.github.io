using Unity.Collections;
using Unity.Entities;

public class FailSuccessActionAuthoring : ActionNodeAuthoring {
    public Node.NodeState state;

    public class FailSuccessActionBaker : Baker<FailSuccessActionAuthoring> {
        public override void Bake(FailSuccessActionAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new FailSuccess {
                state = authoring.state 
            });
        }
    }
}

public struct FailSuccess : IComponentData {
    public Node.NodeState state;
}
