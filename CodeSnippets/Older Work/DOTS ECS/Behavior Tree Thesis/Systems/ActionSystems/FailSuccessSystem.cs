using Unity.Burst;
using Unity.Entities;

partial struct FailSuccessSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRW<Node> node,
            RefRO<FailSuccess> failSuccess,
            Entity entity)
            in SystemAPI.Query<
                RefRW<Node>,
                RefRO<FailSuccess>>().WithEntityAccess()) {

            node.ValueRW.state = failSuccess.ValueRO.state;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
