using System;
using UnityEngine;
using Patterns;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Systems {
    public class FlowFieldController : SceneSingleton<FlowFieldController> {
        [Header("Grid Settings")] 
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;
        [SerializeField] private Vector3 origin = Vector3.zero;
        [SerializeField] private bool debug = true;

        [Header("CostField Settings")] 
        [SerializeField] private LayerMask costFieldLayers;

        [SerializeField] private int extraCost = 3;
        private const int MAX_COST = byte.MaxValue;

        [Header("FlowField Settings")] 
        [SerializeField] private Transform targetLocation;

        public GridSystem2D<Cell> FlowField { get; private set; }

        protected override void Awake() {
            base.Awake();
            InitializeGrid();
            if (!targetLocation) return;

            var cell = FlowField.GetValue(targetLocation.position);
            if (cell == null) {
                Debug.LogError("Target location is not within gridArea");
                return;
            }

            FlowFieldSolver.BuildIntegrationField(FlowField, cell);
            FlowFieldSolver.BuildFlowField(FlowField);
        }

        public GridDirection GetDirection(Vector3 position) {
            var cell = FlowField.GetValue(position);
            return cell.bestDirection;
        }

        private void InitializeGrid() {
            FlowField = GridSystem2D<Cell>.CreateGrid(GridLayout.Horizontal, width, height, cellSize, origin, debug);
            Collider[] overlapColliders = new Collider[32];
            Vector3 cellHalfExtents = Vector3.one * cellSize / 2;
            if (Layers.ExtraCost < 0 || Layers.Impassible < 0)
                throw new InvalidOperationException("Layer doesn't exist");

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++) 
                    FlowField.SetValue(x, y, InitializeCell(x, y, overlapColliders, cellHalfExtents));
        }

        // TODO: there are better options for setting the cost of each cell, change for better optimization
        private Cell InitializeCell(int x, int y, Collider[] overlapColliders, Vector3 cellHalfExtents) {
            var cell = new Cell(x, y);
            var cellPos = FlowField.GetWorldPositionCenter(x, y);
            var size = Physics.OverlapBoxNonAlloc(cellPos, cellHalfExtents, overlapColliders, Quaternion.identity,
                costFieldLayers);
            bool hasIncreasedCost = false;

            if (size == 0) return cell;

            for (int i = 0; i < size; i++) {
                var col = overlapColliders[i];
                if (col.gameObject.layer == Layers.Impassible) {
                    cell.IncreaseCost(MAX_COST);
                    return cell;
                }

                if (!hasIncreasedCost && col.gameObject.layer == Layers.ExtraCost) {
                    cell.IncreaseCost(extraCost);
                    hasIncreasedCost = true;
                }
            }

            return cell;
        }

        // Can remove, Done for testing to create new costfields at runtime
        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetLocation.position = new Vector3(mousePos.x, 0, mousePos.z);
                var cells = FlowField.GetValue(targetLocation.position);
                if (cells == null) {
                    Debug.LogError("Target location is not within gridArea");
                    return;
                }

                FlowFieldSolver.BuildIntegrationField(FlowField, cells);
                FlowFieldSolver.BuildFlowField(FlowField);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (FlowField == null || FlowField.Grid == null) return;

            float halfSize = cellSize / 2f;
            float arrowLength = cellSize * 0.4f; // length of the direction arrow

            foreach (var cell in FlowField.Grid) {
                Vector3 center =
                    FlowField.GetWorldPositionCenter(cell.x, cell.y) + Vector3.up * 0.01f; // slight Y offset

                // 1️⃣ Compute corners of the cell
                Vector3 topLeft = center + new Vector3(-halfSize, 0, halfSize);
                Vector3 topRight = center + new Vector3(halfSize, 0, halfSize);
                Vector3 bottomLeft = center + new Vector3(-halfSize, 0, -halfSize);
                Vector3 bottomRight = center + new Vector3(halfSize, 0, -halfSize);

                // 2️⃣ Color based on cost
                Gizmos.color = Color.Lerp(Color.green, Color.red, cell.bestCost / (float)ushort.MaxValue);

                // 3️⃣ Draw the 4 edges
                Gizmos.DrawLine(topLeft, topRight);
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.DrawLine(bottomRight, bottomLeft);
                Gizmos.DrawLine(bottomLeft, topLeft);

                // 4️⃣ Draw the bestCost label slightly above the cell
                Handles.Label(center + Vector3.up * 0.05f, cell.bestCost.ToString());

                // 5️⃣ Draw the bestDirection arrow
                if (!Equals(cell.bestDirection, GridDirection.None)) {
                    Vector3 dir = new Vector3(cell.bestDirection.Vector.x, 0, cell.bestDirection.Vector.y).normalized;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(center, center + dir * arrowLength);

                    // Optional: draw arrowhead
                    Vector3 right = Quaternion.Euler(0, 150, 0) * dir * (arrowLength * 0.3f);
                    Vector3 left = Quaternion.Euler(0, -150, 0) * dir * (arrowLength * 0.3f);
                    Gizmos.DrawLine(center + dir * arrowLength, center + dir * arrowLength + right);
                    Gizmos.DrawLine(center + dir * arrowLength, center + dir * arrowLength + left);
                }
            }
        }
#endif
    }
}