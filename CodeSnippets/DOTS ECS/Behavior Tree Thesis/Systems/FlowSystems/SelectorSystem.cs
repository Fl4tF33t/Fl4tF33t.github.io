using Unity.Burst;
using Unity.Entities;
using Unity.VisualScripting;

partial struct SelectorSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<Node> node,
            RefRW<Selector> selector,
            DynamicBuffer<Children> children,
            Entity entity)
            in SystemAPI.Query<
                RefRW<Node>,
                RefRW<Selector>,
                DynamicBuffer<Children>>().WithEntityAccess()) {

            if (node.ValueRW.state != Node.NodeState.Running)
                continue;

            if (children.Length == 0) {
                node.ValueRW.state = Node.NodeState.Success;
                continue;
            }

            int index = selector.ValueRO.currentNodeIndex;

            if (!state.EntityManager.IsComponentEnabled<Node>(children[index].Value)) {
                ecb.SetComponentEnabled<Node>(children[index].Value, true);
                continue;
            }

            Node targetChildNode = state.EntityManager.GetComponentData<Node>(children[selector.ValueRO.currentNodeIndex].Value);

            switch (targetChildNode.state) {
                case Node.NodeState.Success:
                    targetChildNode.resetReady = true;
                    ecb.SetComponent(children[selector.ValueRO.currentNodeIndex].Value, targetChildNode);

                    selector.ValueRW.currentNodeIndex = 0;
                    selector.ValueRW.selected = children[index].Value;
                    node.ValueRW.state = Node.NodeState.Success;
                    break;

                case Node.NodeState.Failure:
                    targetChildNode.resetReady = true;
                    selector.ValueRW.currentNodeIndex++;
                    ecb.SetComponent(children[index].Value, targetChildNode);

                    if (selector.ValueRO.currentNodeIndex == children.Length) {
                        selector.ValueRW.currentNodeIndex = 0;
                        node.ValueRW.state = Node.NodeState.Failure;
                    }
                    break;
                case Node.NodeState.Running:
                    break;
            }
        }
    }


    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
