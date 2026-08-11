using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct RandomSelectionSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach ((
            RefRW<Node> node,
            RefRW<RandomSelection> randomSelection,
            DynamicBuffer<EntityOptions> entityOptions,
            Entity entity)
            in SystemAPI.Query<
                RefRW<Node>,
                RefRW<RandomSelection>,
                DynamicBuffer<EntityOptions>>().WithEntityAccess()) {

            if (node.ValueRO.state == Node.NodeState.Running) {
                int index = randomSelection.ValueRW.random.NextInt(0, entityOptions.Length);
                UnityEngine.Debug.Log(index);
                randomSelection.ValueRW.selected = entityOptions[index].Value;
            }



            if (randomSelection.ValueRO.selected != Entity.Null) { 
                node.ValueRW.state = Node.NodeState.Success;
            }

        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
