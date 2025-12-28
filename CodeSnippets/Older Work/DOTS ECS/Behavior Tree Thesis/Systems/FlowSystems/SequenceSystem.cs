using Unity.Burst;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;

partial struct SequenceSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<Node> node,
            RefRW<Sequencer> sequencer,
            DynamicBuffer<Children> children,
            Entity entity)
            in SystemAPI.Query<
                RefRW<Node>,
                RefRW<Sequencer>,
                DynamicBuffer<Children>>().WithEntityAccess()) {

            if (node.ValueRO.state != Node.NodeState.Running)
                continue;

            if (children.Length == 0) {
                node.ValueRW.state = Node.NodeState.Success;
                continue;
            }

            int index = sequencer.ValueRO.currentNodeIndex;

            if (!state.EntityManager.IsComponentEnabled<Node>(children[index].Value)) {
                ecb.SetComponentEnabled<Node>(children[index].Value, true);
                continue;
            }

            Node targetChildNode = state.EntityManager.GetComponentData<Node>(children[sequencer.ValueRO.currentNodeIndex].Value);

            switch (targetChildNode.state) { 
                case Node.NodeState.Success:
                    targetChildNode.resetReady = true;
                    ecb.SetComponent(children[sequencer.ValueRO.currentNodeIndex].Value, targetChildNode);
                    sequencer.ValueRW.currentNodeIndex++;
                    if (sequencer.ValueRO.currentNodeIndex == children.Length) {
                        sequencer.ValueRW.currentNodeIndex = 0;
                        node.ValueRW.state = Node.NodeState.Success;
                    }
                    break;
                case Node.NodeState.Failure:
                    targetChildNode.resetReady = true;
                    sequencer.ValueRW.currentNodeIndex = 0;
                    node.ValueRW.state = Node.NodeState.Failure;
                    ecb.SetComponent(children[index].Value, targetChildNode);
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
