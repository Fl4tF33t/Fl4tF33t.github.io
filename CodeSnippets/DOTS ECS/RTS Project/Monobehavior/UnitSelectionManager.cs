using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

// Manages selection and movement of units
// Monoibehavior script that communicates with the DOTS ECS world
public class UnitSelectionManager : MonoBehaviour {

    public static UnitSelectionManager Instance { get; private set; }

    public event Action OnSelectionAreaStart;
    public event Action OnSelectionAreaEnd;

    private Vector2 selectionStartMousePosition;

    private void Awake() {
        Instance = this; // Set up singleton instance
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // Start selection
            selectionStartMousePosition = Input.mousePosition;
            OnSelectionAreaStart?.Invoke();
        }

        if (Input.GetMouseButtonUp(0)) {
            // End selection
            Vector2 selectionEndMousePosition = Input.mousePosition;

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);

            // Deselect all currently selected units
            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Selected> selectedArray = entityQuery.ToComponentDataArray<Selected>(Allocator.Temp);
            for (int i = 0; i < entityArray.Length; i++) {
                entityManager.SetComponentEnabled<Selected>(entityArray[i], false);
                Selected selected = selectedArray[i];
                selected.onDeselected = true;
                entityManager.SetComponentData(entityArray[i], selected);
            }

            // Determine if it's a single or multiple selection
            Rect selectionArea = GetSelectionAreaRect();
            float selectionAreaSize = selectionArea.width * selectionArea.height;
            float multipleSelectionSizeMin = 40f;
            bool isMultipleSelection = selectionAreaSize > multipleSelectionSizeMin;

            if (isMultipleSelection) {
                // Handle multiple selection
                entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Unit>().WithPresent<Selected>().Build(entityManager);
                entityArray = entityQuery.ToEntityArray(Allocator.Temp);
                NativeArray<LocalTransform> localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

                for (int i = 0; i < localTransformArray.Length; i++) {
                    LocalTransform unitLocalTransform = localTransformArray[i];
                    Vector2 unityScreenPosition = Camera.main.WorldToScreenPoint(unitLocalTransform.Position);
                    if (selectionArea.Contains(unityScreenPosition)) {
                        entityManager.SetComponentEnabled<Selected>(entityArray[i], true);
                        Selected selected = entityManager.GetComponentData<Selected>(entityArray[i]);
                        selected.onSelected = true;
                        entityManager.SetComponentData(entityArray[i], selected);
                    }
                }
            }
            else {
                // Handle single unit selection via raycast
                entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                PhysicsWorldSingleton physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

                UnityEngine.Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastInput raycastInput = new RaycastInput {
                    Start = cameraRay.GetPoint(0f),
                    End = cameraRay.GetPoint(9999f),
                    Filter = new CollisionFilter {
                        BelongsTo = ~0u,
                        CollidesWith = 1u << GameAssets.UNITS_LAYER,
                        GroupIndex = 0
                    }
                };

                if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit)) {
                    if (entityManager.HasComponent<Unit>(raycastHit.Entity) && entityManager.HasComponent<Selected>(raycastHit.Entity)) {
                        entityManager.SetComponentEnabled<Selected>(raycastHit.Entity, true);
                        Selected selected = entityManager.GetComponentData<Selected>(raycastHit.Entity);
                        selected.onSelected = true;
                        entityManager.SetComponentData(raycastHit.Entity, selected);
                    }
                }
            }

            OnSelectionAreaEnd?.Invoke();
        }

        if (Input.GetMouseButton(1)) {
            // Right-click: issue move command to selected units
            Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetMousePosition();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().WithPresent<MoveOverride>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<MoveOverride> moveOverrideArray = entityQuery.ToComponentDataArray<MoveOverride>(Allocator.Temp);

            NativeArray<float3> movePositionArray = GenerateMovePositionArray(mouseWorldPosition, moveOverrideArray.Length);

            for (int i = 0; i < moveOverrideArray.Length; i++) {
                MoveOverride moveOverride = moveOverrideArray[i];
                moveOverride.targetPosition = movePositionArray[i];
                moveOverrideArray[i] = moveOverride;
                entityManager.SetComponentEnabled<MoveOverride>(entityArray[i], true);
            }

            entityQuery.CopyFromComponentDataArray(moveOverrideArray);
        }
    } 
        
    public Rect GetSelectionAreaRect() {
        // Calculate selection rectangle from start and current mouse position
        Vector2 selectionEndMousePosition = Input.mousePosition;

        Vector2 lowerLeftCorner = new Vector2(
            Mathf.Min(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Min(selectionStartMousePosition.y, selectionEndMousePosition.y)
        );

        Vector2 upperRightCorner = new Vector2(
            Mathf.Max(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Max(selectionStartMousePosition.y, selectionEndMousePosition.y)
        );

        return new Rect(
            lowerLeftCorner.x,
            lowerLeftCorner.y,
            upperRightCorner.x - lowerLeftCorner.x,
            upperRightCorner.y - lowerLeftCorner.y
        );
    }

    private NativeArray<float3> GenerateMovePositionArray(float3 targetPosition, int positionCount) {
        // Spread units around target point in rings
        NativeArray<float3> positionArray = new NativeArray<float3>(positionCount, Allocator.Temp);
        if (positionCount == 0) {
            return positionArray;
        }

        positionArray[0] = targetPosition;
        if (positionCount == 1) {
            return positionArray;
        }

        float ringSize = 2.2f;
        int ring = 0;
        int positionIndex = 1;

        while (positionIndex < positionCount) {
            int ringPositionCount = 3 + ring * 2;

            for (int i = 0; i < ringPositionCount; i++) {
                float angle = i * (math.PI2 / ringPositionCount);
                float3 ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ring + 1), 0, 0));
                float3 ringPosition = targetPosition + ringVector;

                positionArray[positionIndex] = ringPosition;
                positionIndex++;

                if (positionIndex >= positionCount) {
                    break;
                }
            }
            ring++;
        }

        return positionArray;
    } 
}
