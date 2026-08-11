using UnityEngine;
using UnityEngine.Events;

namespace Systems {
    public interface IEnergyConsume {
        public bool TryConsumeEnergy(float amount);
    }
    public class EnergyComponent : MonoBehaviour, IEnergyConsume {
        [Header("Energy Config")] 
        [SerializeField, Min(0)] private float maxValue = 100f;
        [SerializeField, Min(0)] private float startingValue = 50f;
        [SerializeField] private bool isRegenerate = true;
        [SerializeField, Min(0)] private float regenRate = 10f; // per second

        [Header("Events")] 
        [SerializeField] private UnityEvent<float> OnValueChanged;
        [SerializeField] private UnityEvent OnEmpty;
        [SerializeField] private UnityEvent OnFull;

        public float Value { get; private set; }
        public bool IsDepleted => Value <= 0f;
        public bool IsFull => Value >= maxValue;

        private bool wasEmpty;
        private bool wasFull;

        private void Awake() {
            Value = Mathf.Clamp(startingValue, 0f, maxValue);
            wasEmpty = IsDepleted;
            wasFull = IsFull;
        }

        private void Update() {
            if (isRegenerate) Regenerate(Time.deltaTime);
        }

        private void Regenerate(float deltaTime) {
            if (IsFull) return;

            ModifyValue(regenRate * deltaTime);
        }

        public bool TryConsumeEnergy(float amount) {
            if (amount <= 0f) return true; // no cost
            if (Value < amount) {
                // Not enough energy
                return false;
            }

            ModifyValue(-amount);
            return true;
        }

        private void ModifyValue(float amount) {
            Value = Mathf.Clamp(Value + amount, 0f, maxValue);
            OnValueChanged?.Invoke(Value);

            if (!wasEmpty && IsDepleted) OnEmpty?.Invoke();
            if (!wasFull && IsFull) OnFull?.Invoke();

            wasEmpty = IsDepleted;
            wasFull = IsFull;
        }
    }
}