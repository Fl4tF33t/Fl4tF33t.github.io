namespace Systems.GameplayStateMachine {
    public class GameFlowController {
        private StateMachine stateMachine = new StateMachine();

        // States
        private Boot boot;
        private MainMenu mainMenu;
        private Playing playing;
        private Paused paused;
        private GameOver gameOver;
        
        // Parameters
        private SceneLoadSignal gameSceneLoad = new SceneLoadSignal();
        private PauseRequestSignal pauseRequestSignal = new PauseRequestSignal();
        private PlayerDeathSignal playerDeathSignal = new PlayerDeathSignal();

        private void Init() {
            CreateStates();
            ConfigureTransitions();
            
            stateMachine.SetState(boot);
        }

        private void CreateStates() {
            boot = new Boot();
            mainMenu = new MainMenu(new MenuController(gameSceneLoad));
            playing = new Playing(new PlayerInput(), new GameSystems(pauseRequestSignal));
            paused = new Paused(new PauseManager());
            gameOver = new GameOver(new ResultsScreen(), new SaveSystem());
        }

        private void ConfigureTransitions() {
            // Boot → Main Menu
            stateMachine.AddTransition(
                boot,
                mainMenu,
                new FuncPredicate((() => boot.IsCompleted))
            );

            // Main Menu → Playing
            stateMachine.AddTransition(
                mainMenu,
                playing,
                // Change accordingly to what you want to trigger the start of the game
                // Alternatively, clicking a button can switch to this state
                new FuncPredicate(() => gameSceneLoad.IsComplete) 
            );

            // Playing → Paused
            stateMachine.AddTransition(
                playing,
                paused,
                new FuncPredicate(() => pauseRequestSignal.IsRequested)
            );
            
            // Paused → Playing
            stateMachine.AddTransition(
                paused,
                playing,
                new FuncPredicate(() => !pauseRequestSignal.IsRequested)
            );

            // Any → Game Over
            stateMachine.AddAnyTransition(
                gameOver,
                new FuncPredicate(() => playerDeathSignal.IsDead)
            );
        }
    }
}