using Unity.Burst;
using Unity.Entities;

partial struct DebugSystem : ISystem
{
    //[BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<Node> node,
            RefRO<Debug> debug,
            Entity entity) 
            in SystemAPI.Query<
                RefRW<Node>,
                RefRO<Debug>>().WithEntityAccess()) {

            if (node.ValueRO.state != Node.NodeState.Running)
                continue;

            if (node.ValueRO.state == Node.NodeState.Running) 
                UnityEngine.Debug.Log(debug.ValueRO.output);

            node.ValueRW.state = Node.NodeState.Success;
        }
    }
}
