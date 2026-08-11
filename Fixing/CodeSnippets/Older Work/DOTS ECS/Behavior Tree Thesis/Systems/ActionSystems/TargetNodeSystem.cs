using Unity.Burst;
using Unity.Entities;

partial struct TargetNodeSystem : ISystem {

    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    //[BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach ((
            RefRO<Target> target, 
            RefRW<Node> node) 
            in SystemAPI.Query<
                RefRO<Target>,
                RefRW<Node>>()) {
            
            //node.ValueRW.state = target.ValueRO.target != Entity.Null ? Node.NodeState.Success : Node.NodeState.Failure;
            node.ValueRW.state = Node.NodeState.Failure;
            UnityEngine.Debug.Log("Target");
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
