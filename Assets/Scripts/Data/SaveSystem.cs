using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveData(GameData gameData)
    {
        BinaryFormatter formatter = new();

        string path = Application.dataPath + "/game.data";
        //string path = Application.persistentDataPath + "/game.data";

        // --- DEBUGGING ---
        string json = JsonUtility.ToJson(gameData);
        using StreamWriter writer = new(path);
        writer.Write(json);
        // -----------------

        //FileStream stream = new(path, FileMode.Create);

        //GameData data = new(gameData.maxUnlockedLevel, gameData.useWASD, gameData.muteMusic, gameData.muteEffects);

        //formatter.Serialize(stream, data);
        //stream.Close();
    }

    public static GameData LoadData()
    {
        string path = Application.dataPath + "/game.data";
        //string path = Application.persistentDataPath + "/game.data";

        if (File.Exists(path))
        {
            // --- DEBUGGING ---
            using StreamReader reader = new(path);
            string json = reader.ReadToEnd();
            GameData data = JsonUtility.FromJson<GameData>(json);
            // -----------------

            //BinaryFormatter formatter = new();
            //FileStream stream = new(path, FileMode.Open);

            //GameData data = formatter.Deserialize(stream) as GameData;
            //stream.Close();

            return data;
        }
        else
        {
            Debug.Log($"[SaveSystem] Save file not found in {path}, creating new one");
            return null;
        }
    }
}
