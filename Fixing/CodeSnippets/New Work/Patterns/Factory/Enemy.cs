using Systems;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Patterns {
    public interface IEnemy {
        public string Name { get; }
        public int Health { get; }
        public void Attack();
    }
    public class Enemy : MonoBehaviour, IEnemy{
        public string Name { get; private set; }
        public int Health { get; private set; }

        public void Initialize(string enemyName, int enemyHealth) {
            // Add logic at after the creation for Initialising 
            Name = enemyName;
            Health = enemyHealth;
        }
        public void Attack() {
            // Deal damage; Trigger animation and sound effects etc
        }

        private void OnEnable() {
            StartCoroutine(Despawn());
        }

        private IEnumerator Despawn() {
            float time = Random.Range(0.2f, 1.5f);
            yield return new WaitForSeconds(time);
            ObjectPoolManager.Instance.Despawn(this);
        }
    }
}