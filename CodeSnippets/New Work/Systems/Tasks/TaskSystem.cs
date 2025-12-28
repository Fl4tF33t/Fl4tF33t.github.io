using System.Collections.Generic;
using UnityEngine;

namespace Systems.Tasks {
    public class TaskSystem : MonoBehaviour {
        [SerializeField] private TaskSO[] tasksSOs;

        private Dictionary<string, ITask> tasks = new();

        private void Awake() {
            // Create runtime tasks
            foreach (var taskSO in tasksSOs) {
                tasks[taskSO.taskID] = new Task(taskSO);
            }

            // Initialize references
            foreach (var task in tasks.Values)
                task.Initialize(tasks);
        }

        public void CompleteTask(string taskId) {
            if (tasks.TryGetValue(taskId, out var task))
                task.Complete();
        }

        public List<ITask> GetAvailableTasks() {
            var available = new List<ITask>();
            foreach (var task in tasks.Values)
                if (task.IsAvailable)
                    available.Add(task);
            return available;
        }
    }
}