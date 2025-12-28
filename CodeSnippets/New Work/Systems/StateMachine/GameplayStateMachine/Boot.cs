using System.Threading.Tasks;

namespace Systems.GameplayStateMachine {
    public class Boot : GameplayState {
        public bool IsCompleted { get; private set; } = false;

        // add any configurable(s) that need to be done here before the main menu screen loads
        // perfect for any async loading 
        public override void OnEnter() {
            // Example: load config, services, save data
           _ = RunAsyncTask();
        }

        private async Task RunAsyncTask() {
            await InitializeAsync();    
            IsCompleted = true;
        }

        private async Task InitializeAsync() {
            // placeholder for real loading
            // can put splash screen or connect to servers etc.
            await Task.Delay(500); 
        }
        
    }
}