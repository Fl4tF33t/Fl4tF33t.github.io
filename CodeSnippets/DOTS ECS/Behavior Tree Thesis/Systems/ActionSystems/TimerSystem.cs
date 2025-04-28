using Unity.Burst;
using Unity.Entities;

partial struct TimerSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach ((
            RefRW<Timer> timer,
            RefRW<Node> node)
            in SystemAPI.Query<
                RefRW<Timer>,
                RefRW<Node>>()) {

            if (node.ValueRW.state == Node.NodeState.Success)
                continue;

            if (timer.ValueRO.Value > 0) {
                node.ValueRW.state = Node.NodeState.Running;
                timer.ValueRW.Value -= SystemAPI.Time.DeltaTime;
                continue;
            }

            timer.ValueRW.Value = timer.ValueRO.timer; 
            node.ValueRW.state = Node.NodeState.Success;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
