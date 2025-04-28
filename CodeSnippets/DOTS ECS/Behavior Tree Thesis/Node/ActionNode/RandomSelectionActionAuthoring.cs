using Unity.Entities;
using UnityEngine;

public class RandomSelectionActionAuthoring : ActionNodeAuthoring {
    public GameObject[] gameObjectOptions;

    public class RandomSelectionActionBaker : Baker<RandomSelectionActionAuthoring> {
        public override void Bake(RandomSelectionActionAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new RandomSelection {
                random = new Unity.Mathematics.Random((uint)entity.Index),
                selected = Entity.Null
            });
            if (authoring.gameObjectOptions != null) {
                var entityBuffer = AddBuffer<EntityOptions>(entity);
                entityBuffer.Capacity = authoring.gameObjectOptions.Length;
                foreach (var option in authoring.gameObjectOptions) {
                    entityBuffer.Add(new EntityOptions {
                        Value = GetEntity(option, TransformUsageFlags.Dynamic)
                    });
                }
            }
        }
    }
}

public struct RandomSelection : IComponentData {
    public Unity.Mathematics.Random random;
    public Entity selected;
}
public struct EntityOptions : IBufferElementData {
    public Entity Value;
}

