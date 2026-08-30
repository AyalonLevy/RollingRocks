using UnityEngine;

public static class SaveSystem
{
    private const string SaveKey = "GameData";

    public static void SaveData(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static GameData LoadData()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<GameData>(json);
        }

        Debug.Log("[SaveSystem] Save not found, creating new one");
        return null;
    }
}
