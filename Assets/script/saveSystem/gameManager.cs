using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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

    private void Update()
    {
        // Fitur Tes Manual menggunakan tombol S
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject player = GameObject.FindGameObjectWithTag("player");
            if (player != null)
            {
                SavePlayerPosition(player.transform, true);
                Debug.Log("<color=green>Sistem: Manual Save via tombol S SUKSES!</color>");
            }
            else
            {
                Debug.LogWarning("Sistem: Gagal manual save. Player dengan tag 'player' tidak ada di scene.");
            }
        }
    }

    public void NewGame()
    {
        data = new GameData();
        SaveGame();
    }

    public void SaveGame()
    {
        if (data == null) data = new GameData();
        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        data = SaveSystem.Load();
        if (data == null) data = new GameData();
    }

    // Fungsi utama penerima instruksi dari AutoSavePlayer maupun interaksi dunia
    public void SavePlayerPosition(Transform playerTransform, bool forceWriteToDisk)
    {
        if (playerTransform == null) return;
        if (data == null) data = new GameData();

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu") return; // Ganti jika nama scene menu utama berbeda

        data.lastScene = currentScene;

        // VALIDASI KRUSIAL: Cari apakah data scene ini sudah pernah terdaftar di list
        ScenePosition existing = data.scenePositions.Find(s => s.sceneName == currentScene);

        if (existing != null)
        {
            // Update data yang sudah ada, JANGAN pakai .Add() lagi agar tidak menumpuk
            existing.x = playerTransform.position.x;
            existing.y = playerTransform.position.y;
            existing.z = playerTransform.position.z;
        }
        else
        {
            // Jika scene benar-benar baru dikunjungi, buat record baru
            ScenePosition newPos = new ScenePosition(currentScene, playerTransform.position);
            data.scenePositions.Add(newPos);
        }

        // Tulis ke storage fisik (JSON) hanya jika diminta (misal interval disk tercapai / pencet S)
        if (forceWriteToDisk)
        {
            SaveGame();
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
        if (data == null || string.IsNullOrEmpty(data.lastScene))
            yield break;

        yield return SceneManager.LoadSceneAsync(data.lastScene);
    }

    // ==========================================
    // NPC SYSTEM (Otomatis mengunci posisi Player)
    // ==========================================
    public bool IsNpcGone(string id) => data != null && data.npcGone.Contains(id);
    public bool IsNpcMet(string id) => data != null && data.npcMet.Contains(id);

    public void SetNpcMet(string id)
    {
        if (data == null) data = new GameData();
        if (!data.npcMet.Contains(id)) data.npcMet.Add(id);

        LockCurrentPlayerState();
    }

    public void SetNpcGone(string id)
    {
        if (data == null) data = new GameData();
        if (!data.npcGone.Contains(id)) data.npcGone.Add(id);

        LockCurrentPlayerState();
    }

    // ==========================================
    // OBJECT UNLOCK SYSTEM (TIRUAN DARI NPC SYSTEM)
    // ==========================================
    public bool IsObjectUnlocked(string id) => data != null && data.objectUnlocked.Contains(id);

    public void SetObjectUnlocked(string id, bool shouldSave)
    {
        if (data == null) data = new GameData();

        if (!data.objectUnlocked.Contains(id))
        {
            data.objectUnlocked.Add(id);

            // Otomatis amankan posisi player saat berinteraksi dengan objek krusial
            if (shouldSave)
            {
                LockCurrentPlayerState();
            }
        }
    }

    private void LockCurrentPlayerState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            SavePlayerPosition(player.transform, true);
        }
        else
        {
            SaveGame();
        }
    }
}