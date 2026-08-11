using UnityEngine;

namespace Patterns {
    public abstract class SpawnStrategy : IStrategy<Vector3> {
        public abstract Vector3 Execute();
    }
}