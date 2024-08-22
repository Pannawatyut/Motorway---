using System.IO;
using UnityEngine;

public static class JsonFileUtility
{
    private static readonly string filePath = Path.Combine(Application.persistentDataPath, "playerData.json");

    public static void SavePlayerName(string playerName)
    {
        PlayerData data = new PlayerData { playerName = playerName };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public static string LoadPlayerName()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data.playerName;
        }
        return string.Empty;
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
    }
}