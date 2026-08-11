using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Patterns {
    public class EnemyFactory : Factory<Enemy> {
        private readonly EnemySO[] enemies;
        private readonly IStrategy<Vector3> spawnStrategy;

        public EnemyFactory(EnemySO[] enemies, IStrategy<Vector3> spawnStrategy) {
            if (enemies == null || enemies.Length == 0)
                throw new ArgumentException("EnemyFactory requires at least one EnemySO.");

            this.enemies = enemies;
            this.spawnStrategy = spawnStrategy ?? throw new ArgumentNullException(nameof(spawnStrategy));
        }
        
        public override Enemy Create() {
            EnemySO enemyData = enemies[Random.Range(0, enemies.Length)];

            GameObject go = Object.Instantiate(enemyData.prefab, spawnStrategy.Execute(), Quaternion.identity);
            if (!go.TryGetComponent(out Enemy enemy))
                throw new InvalidOperationException ($"Enemy prefab '{enemyData.name}' is missing Enemy component.");

            enemy.Initialize(enemyData.enemyName, enemyData.enemyHealth);
            enemy.gameObject.SetActive(false);
            return enemy;
        }
    }
}