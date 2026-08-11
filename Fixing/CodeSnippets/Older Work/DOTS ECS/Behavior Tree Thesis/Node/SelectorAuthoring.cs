using UnityEngine;
using Unity.Entities;

public class SelectorAuthoring : MonoBehaviour {

    public class SelectorAuthoringBaker : Baker<SelectorAuthoring> {
        public override void Bake(SelectorAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            DynamicBuffer<NodeBuffer> selector = AddBuffer<NodeBuffer>(entity);
        }
    }
}

public struct NodeBuffer : IBufferElementData {
    public Node Value;
}
