using UnityEngine;

namespace Systems {
    [System.Serializable]
    public struct PlayerData {
        public int level;
        public int health;
        public int gold;
        public Vector3 position;
    }
    class PlayerController : MonoBehaviour, ISaveable {
        [SerializeField] private PlayerData playerData = new PlayerData();
        public string visibleID;
        [SerializeField, HideInInspector] private string uniqueID;
        private void Start() {
            visibleID = UniqueID;
            SaveDataManager.Instance.RegisterSaveable(this);
        }

        public string UniqueID {
            get {
                if (string.IsNullOrEmpty(uniqueID))
                    uniqueID = gameObject.name + "_" + nameof(PlayerData);
                
                return uniqueID;
            }
        }

        public object GetSaveData() {
            playerData.gold = 4;
            playerData.position = Vector3.up;
            playerData.level = 2;
            playerData.health = 89;
            return playerData;
        }

        public void LoadData(object data) => playerData = (PlayerData)data;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) SaveDataManager.Instance.SaveData(this);
            if (Input.GetKeyDown(KeyCode.D)) SaveDataManager.Instance.DeleteAllData();
            if (Input.GetKeyDown(KeyCode.L)) SaveDataManager.Instance.LoadData(this);
        }
    }
}
