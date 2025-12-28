using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct FollowTargetSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach((
            var localTransform,
            var target,
            var node)
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<Target>,
                RefRW<Node>>()) {

        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
