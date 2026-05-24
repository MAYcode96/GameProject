using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameData data;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGame();
    }

    public void NewGame()
    {
        data = new GameData();
        SaveGame();
    }

    public void SaveGame()
    {
        if (data == null)
            data = new GameData();

        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        data = SaveSystem.Load();

        if (data == null)
            data = new GameData();
    }

    public bool IsNpcGone(string id)
    {
        return data != null && data.npcGone.Contains(id);
    }

    public bool IsNpcMet(string id)
    {
        return data != null && data.npcMet.Contains(id);
    }

    public void SetNpcMet(string id)
    {
        if (data == null) data = new GameData();

        if (!data.npcMet.Contains(id))
            data.npcMet.Add(id);
    }

    public void SetNpcGone(string id)
    {
        if (data == null) data = new GameData();

        if (!data.npcGone.Contains(id))
            data.npcGone.Add(id);
    }
}