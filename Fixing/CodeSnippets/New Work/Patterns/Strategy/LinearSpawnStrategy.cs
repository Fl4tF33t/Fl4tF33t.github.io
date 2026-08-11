using UnityEngine;

namespace Patterns {
    public class LinearSpawnStrategy : SpawnStrategy {
        private readonly Vector3 start;
        private readonly Vector3 end;
        private readonly int maxCount;
        private int index = 0;

        public LinearSpawnStrategy(Vector3 start, Vector3 end, int maxCount = 5) {
            this.start = start;
            this.end = end;
            this.maxCount = maxCount;
        }
        public override Vector3 Execute() {
            float t = index / (float)(maxCount - 1);
            index++;
            return Vector3.Lerp(start, end, Mathf.Clamp01(t));
        }
    }
}