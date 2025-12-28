using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(NodeAuthoring))]
public class BehaviourTreeAuthoring : MonoBehaviour {
    public NodeAuthoring child;
    public bool repeat;
    public class BehaviourTreeAuthoringBaker : Baker<BehaviourTreeAuthoring> {
        public override void Bake(BehaviourTreeAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BehaviourTreeRoot {
                repeat = authoring.repeat,
                child = authoring.child != null ? GetEntity(authoring.child, TransformUsageFlags.None) : Entity.Null
            });
            AddBuffer<FloatBlackboard>(entity); 
        }
    }
}

public struct BehaviourTreeRoot : IComponentData {
    public bool initialInit;
    public bool repeat;
    public Entity child;
}
public struct FloatBlackboard : IBufferElementData {
    public FixedString32Bytes element;
    public float3 Value;
}
public struct Float2Blackboard : IBufferElementData {
    public float3 Value;
}
public struct Float3Blackboard : IBufferElementData {
    public float3 Value;
}
