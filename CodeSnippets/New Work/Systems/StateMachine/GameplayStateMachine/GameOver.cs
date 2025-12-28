namespace Systems.GameplayStateMachine {
    public class ResultsScreen {
        public void Show(){}
    }

    public class SaveSystem {
        public void Commit(){}
    }
    
    public class GameOver : GameplayState {
        private readonly ResultsScreen results;
        private readonly SaveSystem save;

        public GameOver(ResultsScreen results, SaveSystem save) {
            this.results = results;
            this.save = save;
        }

        public override void OnEnter() {
            save.Commit();
            results.Show();
        }
    }
}