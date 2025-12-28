using System.Collections.Generic;
using UnityEngine;

namespace Systems.Tasks {
    public class Task : ITask {
        private readonly TaskSO data;

        private readonly List<ITask> prerequisites = new();
        private readonly List<ITask> unlocks = new();
        public bool IsCompleted { get; private set; }
        public bool IsAvailable => !IsCompleted && prerequisites.TrueForAll(t => t.IsCompleted);
        public Task(TaskSO data) {
            this.data = data;
        }

        public void Initialize(Dictionary<string, ITask> taskLookup) {
            // Link runtime tasks for prerequisites and unlocks
            foreach (var prereqSO in data.prerequisites)
                if (taskLookup.TryGetValue(prereqSO.taskID, out var t))
                    prerequisites.Add(t);

            foreach (var unlockSO in data.unlocks)
                if (taskLookup.TryGetValue(unlockSO.taskID, out var t))
                    unlocks.Add(t);
        }

        public void Complete() {
            if (IsCompleted) return;

            IsCompleted = true;
            foreach (var task in unlocks) {
                if (task.IsAvailable)
                    Debug.Log($"{task} is now available!");
            }
        }
    }
}