using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct InitializeNodeSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRO<Node> node,
            Entity entity)
            in SystemAPI.Query<
                RefRO<Node>>().WithEntityAccess()) {

            //if (node.ValueRO.state == Node.NodeState.Running)
            //    continue;

            //if (node.ValueRO.readByParent) {
            //    ecb.SetComponentEnabled<Node>(entity, false);
            //    UnityEngine.Debug.Log("Turn off node");
            //}
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
