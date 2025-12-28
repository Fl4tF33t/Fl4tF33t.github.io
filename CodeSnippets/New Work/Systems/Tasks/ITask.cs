using System.Collections.Generic;

namespace Systems.Tasks {
    public interface ITask {
        public void Initialize(Dictionary<string, ITask> tasks);
        public void Complete();
        public bool IsCompleted { get; }
        public bool IsAvailable { get; }
    }
}