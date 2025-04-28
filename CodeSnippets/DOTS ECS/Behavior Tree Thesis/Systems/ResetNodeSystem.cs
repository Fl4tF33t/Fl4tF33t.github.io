using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct ResetNodeSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRO<Node> node,
            Entity entity)
            in SystemAPI.Query<
                RefRO<Node>>().WithEntityAccess()) {

            if (node.ValueRO.CanResetNode()) {
                ecb.SetComponent(entity, new Node {
                    state = Node.NodeState.Running,
                });
                ecb.SetComponentEnabled<Node>(entity, false);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
