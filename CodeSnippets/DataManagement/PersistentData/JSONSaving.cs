using System;
using System.IO;
using UnityEngine;

public class JSONSaving : Singleton<JSONSaving>
{
    [SerializeField]
    private PlayerData playerData = new PlayerData();

    public PlayerData PlayerData
    {
        get { return playerData; }
        set { playerData = value; SaveData(playerData); Debug.Log("Datat was saved"); }
    }

    private string persistentPath;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        if (File.Exists(persistentPath))
        {
            LoadData();
            Debug.Log("Loaded existing");
        }
        else
        {
            PlayerData = playerData;
            Debug.Log("Created new");
        }
    }

    private void SaveData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(persistentPath, json);
    }

    private void LoadData()
    {
        string json = File.ReadAllText(persistentPath);
        playerData = JsonUtility.FromJson<PlayerData>(json);
    }
    
}
