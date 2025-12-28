using Unity.Collections;
using Unity.Entities;
using UnityEngine;

//This is used for testing purposes
//Have to use systembase to enable the debug setting
public class DebugActionAuthoring : ActionNodeAuthoring {
    public string output;

    public class DebugActionBaker : Baker<DebugActionAuthoring> {
        public override void Bake(DebugActionAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Debug {
                output = authoring.output != null ? authoring.output : "Debug Test"
            });
        }
    }
}

public struct Debug : IComponentData {
    public FixedString32Bytes output;
}
