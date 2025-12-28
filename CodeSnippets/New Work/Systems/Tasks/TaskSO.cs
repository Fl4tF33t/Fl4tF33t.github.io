using UnityEngine;

namespace Systems {
    [CreateAssetMenu(fileName = "TaskSO", menuName = "Scriptable Objects/TaskSO")]
    public class TaskSO : ScriptableObject {
        public string taskID;
        public string descrption;

        // Tasks that must be completed before this one unlocks
        public TaskSO[] prerequisites;

        // Tasks that this task unlocks upon completion
        public TaskSO[] unlocks;
    }
}