using System;
using System.Collections.Generic;
using Patterns;
using UnityEngine;

namespace Systems {
    
    [Serializable]
    public struct Data {
        public string uniqueID;
        public string data;
    }

    [Serializable]
    public struct SaveData {
        public List<Data> entries;
    }
    
    // If Using the PlayerPrefSaveSystem, no need for a DataSerializer
    // Note: can only use int, float, string, or Dictionary for this
    
    // If using FileSaveSystem, can change between serializers
    // Note: cannot serialize Dictionaries 
    // If wanting one large file rather than smaller one, use the provided structs put into a List and modify script
    public class SaveDataManager : PersistentSingleton<SaveDataManager> {
        private readonly IDataSerializer serializer = new JsonSerializer(); //use when needed
        
        private readonly List<IPlayerPref> playerPrefs = new();
        private readonly List<ISaveable> saveables = new();

        private ISaveSystem fileSaveSystem;
        protected override void Awake() {
            base.Awake();
            string directory = Application.persistentDataPath;
            fileSaveSystem = new FileSaveSystem(directory, serializer);
        }

        public void RegisterSaveable(IPlayerPref saveable) {
            if (!playerPrefs.Contains(saveable))
                playerPrefs.Add(saveable);
        }

        public void RegisterSaveable(ISaveable saveable) {
            if (!saveables.Contains(saveable))
                saveables.Add(saveable);
        }
        public void UnregisterSaveable(IPlayerPref saveable) => playerPrefs.Remove(saveable);
        public void UnregisterSaveable(ISaveable saveable) => saveables.Remove(saveable);

        public void SaveData(ISaveable saveable) => fileSaveSystem.Save(saveable.UniqueID, saveable.GetSaveData());
        public void SaveAllData() {
            foreach (var saveable in playerPrefs) {
                saveable.Save();
            }
            foreach (var saveable in saveables) {
                fileSaveSystem.Save(saveable.UniqueID, saveable.GetSaveData());
            }
        }

        public void LoadData(ISaveable saveable) {
            var dataType = saveable.GetSaveData().GetType();
            if (fileSaveSystem.Load(saveable.UniqueID, dataType, out var data)) {
                saveable.LoadData(data);
            }
        }
        public void LoadAllData() {
            foreach (var saveable in playerPrefs) 
                saveable.Load();

            foreach (var saveable in saveables) {
                var dataType = saveable.GetSaveData().GetType();
                if (fileSaveSystem.Load(saveable.UniqueID, dataType, out var data))
                    saveable.LoadData(data);
            }
        }
        
        public void DeleteAllData() {
            PlayerPrefs.DeleteAll();
            fileSaveSystem.DeleteAll();
        }
    }
}