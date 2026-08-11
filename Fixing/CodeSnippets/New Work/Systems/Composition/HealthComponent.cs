using UnityEngine;
using UnityEngine.Events;

namespace Systems {
    public interface IDamage {
        public void Damage(int damage);
    }
    
    public class HealthComponent : MonoBehaviour, IDamage {
        [Header("Health Config")]
        [SerializeField, Min(0)] private int maxValue = 100;
        [SerializeField, Min(0)] private int startingValue = 50;
        
        [Header("Events")]
        // Or use any other system that you have for communication
        [SerializeField] private UnityEvent<int> OnValueChanged;
        [SerializeField] private UnityEvent OnDeath;
        public bool IsAlive => Value > 0;
        public int Value { get; private set; }
        private void OnValidate() => startingValue = Mathf.Clamp(startingValue, 0, maxValue);
        private void Awake() => Value = Mathf.Clamp(startingValue, 0, maxValue);

        private void Start() => OnValueChanged?.Invoke(Value);

        public void Damage(int damage) {
            if (!IsAlive || damage <= 0) return;

            Value = Mathf.Max(Value - damage, 0);
            OnValueChanged?.Invoke(Value);

            if (Value == 0) OnDeath.Invoke();
        }
    }
}