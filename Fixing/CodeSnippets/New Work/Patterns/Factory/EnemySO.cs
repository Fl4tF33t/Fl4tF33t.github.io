using UnityEngine;

namespace Patterns {
    [CreateAssetMenu(menuName = "Create EnemySO", fileName = "EnemySO", order = 0)]
    public class EnemySO : ScriptableObject {
        public GameObject prefab;
        public string enemyName;
        public int enemyHealth;
    }
}