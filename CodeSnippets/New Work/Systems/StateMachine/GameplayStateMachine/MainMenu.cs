using System.Threading.Tasks;
using UnityEngine;

namespace Systems.GameplayStateMachine {
    public class SceneLoadSignal {
        public bool IsComplete { get; private set; } = false;
        public void MarkComplete() => IsComplete = true;
        public void Reset() => IsComplete = false;
    }
    public class MenuController {
        private readonly SceneLoadSignal loadSignal;

        public MenuController(SceneLoadSignal sceneLoadSignal) {
            loadSignal = sceneLoadSignal;
        }
        public void Show(){}
        public void Hide(){}

        public async Task StartGame() {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
            loadSignal.MarkComplete();
        }
    }
    public class MainMenu : GameplayState {
        private readonly MenuController menu;
        
        public MainMenu(MenuController menu) {
            this.menu = menu;
        }

        public override void OnEnter() => menu.Show();
        public override void OnExit() => menu.Hide();
        
    }
}