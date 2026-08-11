using UnityEngine;
using Unity.Entities;

//Abstract class for all flow nodes to inherit from, adds the Node Authoring component
[RequireComponent(typeof(NodeAuthoring))]
public class FlowNodeAuthoring : MonoBehaviour {
    public NodeAuthoring[] children;

    public class FlowNodeAuthoringBaker : Baker<FlowNodeAuthoring> {
        public override void Bake(FlowNodeAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            DynamicBuffer<Children> children =  AddBuffer<Children>(entity);

            foreach (var child in authoring.children) {
                children.Add(new Children {
                    Value = GetEntity(child, TransformUsageFlags.None)
                });
            }
        }
    }
}

public struct Children : IBufferElementData {
    public Entity Value;
}
