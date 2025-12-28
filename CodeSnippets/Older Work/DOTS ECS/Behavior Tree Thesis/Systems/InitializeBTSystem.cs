using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct InitializeBTSystem : ISystem {

    [BurstCompile]
    public void OnCreate(ref SystemState state) {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((
            RefRO<BehaviourTreeRoot> btRoot,
            Entity entity)
                in SystemAPI.Query<
            RefRO<BehaviourTreeRoot>>()
            .WithEntityAccess()) {

            //if (btRoot.ValueRO.child == Entity.Null) {
            //    UnityEngine.Debug.Log("No Available Child");
            //    continue;
            //}

            //if (!btRoot.ValueRO.initialInit) {
            //    //set the init true
            //    BehaviourTreeRoot temp = btRoot.ValueRO;
            //    temp.initialInit = true;
            //    ecb.SetComponent(entity, temp);

            //    //turn on the node and child node
            //    ecb.SetComponentEnabled<Node>(entity, true);
            //    ecb.SetComponentEnabled<Node>(btRoot.ValueRO.child, true);
            //    UnityEngine.Debug.Log("First time bt init");
            //    continue;
            //}

            //if (btRoot.ValueRO.repeat && !state.EntityManager.IsComponentEnabled<Node>(entity)) {
            //    ecb.SetComponentEnabled<Node>(entity, true);
            //    ecb.SetComponentEnabled<Node>(btRoot.ValueRO.child, true);
            //    UnityEngine.Debug.Log("Repeat bt init");
            //    continue;
            //}

            //if (!state.EntityManager.IsComponentEnabled<Node>(entity)) {
            //    UnityEngine.Debug.Log("No Repeat");
            //    continue;
            //}

            //Node childNode = state.EntityManager.GetComponentData<Node>(btRoot.ValueRO.child);
            //Node btNode = state.EntityManager.GetComponentData<Node>(entity);

            //switch (childNode.state) {
            //    case Node.NodeState.Failure:
            //        UnityEngine.Debug.Log("bt failure");
            //        childNode.resetReady = true;
            //        btNode.state = Node.NodeState.Failure;
            //        btNode.resetReady = true;
            //        break;
            //    case Node.NodeState.Success:
            //        UnityEngine.Debug.Log("bt success");
            //        childNode.resetReady = true;
            //        btNode.state = Node.NodeState.Success;
            //        btNode.resetReady = true;
            //        break;
            //    case Node.NodeState.Running:
            //        UnityEngine.Debug.Log("bt running");
            //        btNode.state = Node.NodeState.Running;
            //        break;
            //}
            //ecb.SetComponent(entity, btNode);
            //ecb.SetComponent(btRoot.ValueRO.child, childNode);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) {

    }
}
