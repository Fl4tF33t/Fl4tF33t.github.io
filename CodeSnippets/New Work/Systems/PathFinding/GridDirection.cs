using UnityEngine;

namespace Systems {
    public readonly struct GridDirection {
        public Vector2Int Vector { get; }

        private GridDirection(int x, int y) {
            Vector = new Vector2Int(x, y);
        }

        public static implicit operator Vector2Int(GridDirection dir) => dir.Vector;

        public static GridDirection FromVector(Vector2Int vector) {
            foreach (var dir in AllDirections) {
                if (dir.Vector == vector)
                    return dir;
            }
            return None;
        }

        // Directions
        public static readonly GridDirection None = new(0, 0);
        public static readonly GridDirection North = new(0, 1);
        public static readonly GridDirection South = new(0, -1);
        public static readonly GridDirection East = new(1, 0);
        public static readonly GridDirection West = new(-1, 0);
        public static readonly GridDirection NorthEast = new(1, 1);
        public static readonly GridDirection NorthWest = new(-1, 1);
        public static readonly GridDirection SouthEast = new(1, -1);
        public static readonly GridDirection SouthWest = new(-1, -1);

        public static readonly GridDirection[] CardinalDirections = {
            North, South, East, West
        };

        public static readonly GridDirection[] AllDirections = {
            None,
            North, South, East, West,
            NorthEast, NorthWest, SouthEast, SouthWest
        };
    }
}