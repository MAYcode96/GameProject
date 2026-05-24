using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string tempPath = path + ".tmp";

        File.WriteAllText(tempPath, json);

        if (File.Exists(path))
            File.Delete(path);

        File.Move(tempPath, path);

        Debug.Log("Game Saved: " + path);
    }

    public static GameData Load()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No Save File Found, Creating New Data");
            return new GameData();
        }

        string json = File.ReadAllText(path);

        if (string.IsNullOrEmpty(json))
            return new GameData();

        GameData data = JsonUtility.FromJson<GameData>(json);

        return data ?? new GameData();
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}