using System.Collections.Generic;

namespace Systems.GameplayStateMachine {
    // Dummy Pause manager to showcase different game state implementation
    
    public class PauseRequestSignal {
        public bool IsRequested { get; private set; }

        public void Request() => IsRequested = true;
        public void Clear() => IsRequested = false;
    }
    
    public interface IPausable {
        public void Pause();
        public void Resume();
    }

    public class PauseManager { 
        private HashSet<IPausable> pauseSet = new();
        public void Register(IPausable pausable) => pauseSet.Add(pausable);
        public void Unregister(IPausable pausable) => pauseSet.Remove(pausable);

        public void PauseAll() {
            foreach (var pause in pauseSet)
                pause.Pause();
        }
        
        public void ResumeAll() {
            foreach (var pause in pauseSet)
                pause.Resume();
        }
    }
    public class Paused : GameplayState {
        private readonly PauseManager pauseManager;
        public Paused(PauseManager manager) {
            pauseManager = manager;
        }
        public override void OnEnter() => pauseManager.PauseAll();
        public override void OnExit() => pauseManager.ResumeAll();
    }
}