using UnityEngine;
using Unity.Entities;

//Think of this class as an abstract that other nodes will require as a component to be considered a node
public class NodeAuthoring : MonoBehaviour {
    public class NodeAuthoringBaker : Baker<NodeAuthoring> {
        public override void Bake(NodeAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Node { 
                state = Node.NodeState.Running
            });
            SetComponentEnabled<Node>(entity, false);
            AddComponent<BTRootRef>(entity);
            GameObject parent = GetComponentInParent<BlackboardAuthoring>().gameObject;
            if (parent != null) {
                UnityEngine.Debug.Log("There is");
                Entity ent = GetEntity(parent, TransformUsageFlags.None);
                BTRootRef bTRootRef = new BTRootRef {
                    blackboardRef = ent
                };
                SetComponent(entity, bTRootRef);
            }
        }
    }
}

//public node struct that indiciates the state of the given node
public struct Node : IComponentData, IEnableableComponent {
    public enum NodeState : byte {
        Failure,
        Success,
        Running,
    }
    public NodeState state;
    public bool resetReady;

    public bool CanResetNode() {
        if (state != NodeState.Running) {
            return resetReady;
        }

        return false;
    }
}

public struct BTRootRef : IComponentData {
    public Entity blackboardRef;
}
