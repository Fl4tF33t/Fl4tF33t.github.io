using UnityEngine;

namespace Patterns {
    public class OnCircleEdgeSpawnStrategy : SpawnStrategy {
        private readonly Vector3 center;
        private readonly float radius;
        private readonly int maxCount;
        private int index = 0;
        public OnCircleEdgeSpawnStrategy(Vector3 center, float radius, int maxCount = 5) {
            this.center = center;
            this.radius = radius;
            this.maxCount = maxCount;
        }

        public override Vector3 Execute() {
            float angleStep = 360f / maxCount;
            float angle = angleStep * index;

            index = (index + 1) % maxCount;

            float radians = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(radians) * radius;
            float z = Mathf.Sin(radians) * radius;

            return center + new Vector3(x, 0f, z);
        }
    }
}