namespace Systems.GameplayStateMachine {
    public class PlayerDeathSignal {
        public float Health { get; private set; }
        public bool IsDead => Health <= 0;
    }
    public class PlayerInput { // change with the Player Input system that you are using
        // for PlayerInputAction, recommend using a ScriptableObject Input reader that takes all the IPlayerActions Callbacks
        // Enable and Disable different parts at runtime
        public void Enable(){}
        public void Disable(){}
    }

    public class GameSystems {
        private readonly PauseRequestSignal pauseRequestSignal;
        public GameSystems(PauseRequestSignal pauseRequestSignal) {
            this.pauseRequestSignal = pauseRequestSignal;
        }
        public void OpenMenuUI() => pauseRequestSignal.Request();
        public void CloseMenuUI() => pauseRequestSignal.Clear();
        public void EnableGameplay(){}
        // optionally, can have flag to start tutorial on first time or scene based
    }

    public class Playing : GameplayState {
        private readonly PlayerInput input;
        private readonly GameSystems systems;

        public Playing(PlayerInput input, GameSystems systems) {
            this.input = input;
            this.systems = systems;
        }

        public override void OnEnter() {
            input.Enable();
            systems.EnableGameplay();
        }

        public override void OnExit() {
            input.Disable();
            systems.CloseMenuUI();
        }
    }
    
}