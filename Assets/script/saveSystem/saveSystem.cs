using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(GameData data)
    {
        if (data == null) return;

        string json = JsonUtility.ToJson(data, true);
        string tempPath = path + ".tmp";

        try
        {
            File.WriteAllText(tempPath, json);

            if (File.Exists(path))
                File.Delete(path);

            File.Move(tempPath, path);
            Debug.Log($"[SaveSystem] Berhasil menulis file ke disk: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveSystem] Gagal menulis file JSON: {e.Message}");
        }
    }

    public static GameData Load()
    {
        if (!File.Exists(path))
        {
            Debug.Log("[SaveSystem] File save tidak ditemukan. Membuat data baru.");
            return new GameData();
        }

        try
        {
            string json = File.ReadAllText(path);

            if (string.IsNullOrEmpty(json))
                return new GameData();

            GameData data = JsonUtility.FromJson<GameData>(json);
            return data ?? new GameData();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveSystem] File corrupt atau gagal dibaca, membuat data darurat: {e.Message}");
            return new GameData();
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}