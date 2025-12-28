using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(FlowNodeAuthoring))]
public class SelectorFlowNode : MonoBehaviour {
    public class SelecetorFlowNodeBaker : Baker<SelectorFlowNode> {
        public override void Bake(SelectorFlowNode authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<Selector>(entity);
        }
    }
}

public struct Selector : IComponentData {
    public int currentNodeIndex;
    public Entity selected;
}
