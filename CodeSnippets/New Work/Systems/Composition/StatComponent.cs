using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Systems {
    public enum StatType {
        Strength,
        Agility,
        Intelligence,
    }
    
    [Serializable]
    public struct Stat {
        public StatType type;
        public float baseValue;
        public float currentValue;

        public void Reset() => currentValue = baseValue;
    }

    public class StatsComponent : MonoBehaviour {
        // ensure there is only one type of each stat
        [Header("Base Stats")] 
        [SerializeField] private Stat[] stats;

        [Header("Events")] 
        [SerializeField] private UnityEvent<Stat> OnStatChanged;
        
        private Dictionary<StatType, Stat> statDict = new();

        private void Awake() {
            foreach (Stat stat in stats) {
                statDict.Add(stat.type, stat);
            }
        }

        // Access by name
        public float GetStat(StatType type) {
            if (statDict.TryGetValue(type, out Stat stat)) return stat.currentValue;

            throw new Exception("Stat type doesn't match");
        }

        public void ModifyStat(StatType type, float amount) {
            if (statDict.TryGetValue(type, out Stat stat)) {
                stat.currentValue += amount;
                OnStatChanged?.Invoke(stat);
                return;
            }

            throw new Exception("Stat type doesn't match");
        }

        public void ResetStats() {
            foreach (var stat in statDict.Values) {
                stat.Reset();
                OnStatChanged?.Invoke(stat);
            }
        }
    }
}