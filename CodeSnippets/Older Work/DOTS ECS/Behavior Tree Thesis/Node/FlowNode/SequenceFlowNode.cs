using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(FlowNodeAuthoring))]
public class SequenceFlowNode : MonoBehaviour {

    public class SequenceFlowNodeBaker : Baker<SequenceFlowNode> {
        public override void Bake(SequenceFlowNode authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<Sequencer>(entity);
        }
    }
}

public struct Sequencer : IComponentData {
    public int currentNodeIndex;
}
