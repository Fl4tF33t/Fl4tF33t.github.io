using UnityEngine;

namespace Systems {
    // Add this to classes that need PlayerPref Capabilities
    public class PlayerPrefSettings : MonoBehaviour, IPlayerPref {
        // use this as a simple system that you want to save player prefs
        // Note, does not work well with multiple instances as the key will be duplicated
        // Best case use for player settings like MouseSens, Graphic settings, etc.
        
        private string playerName = "John Doe";
        private float mouseSensitivity = 21.3f;
        private int FOV = 100;

        private void Start() => SaveDataManager.Instance.RegisterSaveable(this);
        public void Save() {
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);
            PlayerPrefs.SetInt("FieldOfView", FOV);
            PlayerPrefs.Save();
        }

        public void Load() {
            playerName = PlayerPrefs.GetString("PlayerName", playerName);
            mouseSensitivity  = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);
            FOV = PlayerPrefs.GetInt("FieldOfView", FOV);
        }
    }
}