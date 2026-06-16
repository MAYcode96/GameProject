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

    private void Start()
    {
        StartCoroutine(ApplyAfterSceneReady());
    }

    private IEnumerator ApplyAfterSceneReady()
    {
        yield return null;
        ApplyObjectStates();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyObjectStates();
    }

    // =========================
    // SAVE / LOAD
    // =========================

    public void NewGame()
    {
        data = new GameData();
        SaveGame();
    }

    public void SaveGame()
    {
        if (data == null) data = new GameData();

        // (AMBIL DATA INVENTORY)
        InventoryController inventoryController = FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);;
        if (inventoryController != null)
        {
            data.inventorySaveData = inventoryController.GetInventoryItems();
        }

        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        data = SaveSystem.Load();
        if (data == null) data = new GameData();

        // (PASANG DATA INVENTORY)
        InventoryController inventoryController = FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);
        if (inventoryController != null && data.inventorySaveData != null)
        {
            inventoryController.SetInventoryItems(data.inventorySaveData);
        }
    }

    // =========================
    // OBJECT SYSTEM
    // =========================

    public bool IsObjectUnlocked(string id)
    {
        return data != null && data.objectUnlocked.Contains(id);
    }
    public void SetObjectUnlocked(string id, bool shouldSave)
    {
        if (data == null) data = new GameData();

        if (!data.objectUnlocked.Contains(id))
        {
            data.objectUnlocked.Add(id);
            Debug.Log("[GameManager] ID BERHASIL DISIMPAN: " + id);

            LockCurrentPlayerState();

            if (shouldSave)
                SaveGame();

            ApplyObjectStates();
        }
    }

    //findChecker
    public void ApplyObjectStates()
    {
        if (data == null) return;

        ObjectUnlockChecker[] checkers =
            Object.FindObjectsByType<ObjectUnlockChecker>(FindObjectsSortMode.None);

        foreach (ObjectUnlockChecker checker in checkers)
        {
            if (checker != null)
                checker.EvaluateState();
        }

        // Cari InventoryController di scene yang baru saja terbuka
        InventoryController inventoryController = FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);
        
        // Jika UI Inventory ada di scene ini, masukkan data item yang tersimpan
        if (inventoryController != null && data.inventorySaveData != null)
        {
            inventoryController.SetInventoryItems(data.inventorySaveData);
            Debug.Log("[GameManager] INVENTORY BERHASIL DI-LOAD");
        }
        // -------------------------------

        Debug.Log("[GameManager] SEMUA STATUS CHECKER TELAH DIPERBARUI");
    }

    // =========================
    // PLAYER SAVE
    // =========================
    public void SavePlayerPosition(Transform playerTransform, bool forceWriteToDisk)
    {
        if (playerTransform == null) return;
        if (data == null) data = new GameData();

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu") return;

        data.lastScene = currentScene;

        ScenePosition existing = data.scenePositions.Find(s => s.sceneName == currentScene);

        if (existing != null)
        {
            existing.x = playerTransform.position.x;
            existing.y = playerTransform.position.y;
            existing.z = playerTransform.position.z;
        }
        else
        {
            data.scenePositions.Add(
                new ScenePosition(currentScene, playerTransform.position)
            );
        }

        if (forceWriteToDisk)
            SaveGame();
    }

    private void LockCurrentPlayerState()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");

        if (player != null)
            SavePlayerPosition(player.transform, true);
        else
            SaveGame();
    }

    // =========================
    // NPC SYSTEM (TETAP)
    // =========================

    public bool IsNpcGone(string id)
        => data != null && data.npcGone.Contains(id);

    public bool IsNpcMet(string id)
        => data != null && data.npcMet.Contains(id);

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

    // =========================
    // SCENE CONTINUE
    // =========================

    public void ContinueGame()
    {
        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
        if (data == null || string.IsNullOrEmpty(data.lastScene))
            yield break;

        yield return SceneManager.LoadSceneAsync(data.lastScene);
        yield return null;

        ApplyObjectStates();
    }

    // =========================
    // NPC POSITION SAVE
    // =========================

    public void SaveNPCPosition(string npcID, Transform npcTransform)
    {
        if (npcTransform == null) return;
        if (data == null) data = new GameData();

        string currentScene = SceneManager.GetActiveScene().name;

        NPCSaveData existing =
            data.npcPositions.Find(n =>
                n.npcID == npcID &&
                n.sceneName == currentScene
            );

        if (existing != null)
        {
            existing.x = npcTransform.position.x;
            existing.y = npcTransform.position.y;
            existing.z = npcTransform.position.z;
        }
        else
        {
            data.npcPositions.Add(
                new NPCSaveData(
                    npcID,
                    currentScene,
                    npcTransform.position
                )
            );
        }

        SaveGame();
    }

    public bool HasNPCPosition(string npcID)
    {
        if (data == null) return false;

        string currentScene = SceneManager.GetActiveScene().name;

        return data.npcPositions.Exists(n =>
            n.npcID == npcID &&
            n.sceneName == currentScene
        );
    }

    public Vector3 GetNPCPosition(string npcID)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        NPCSaveData npc =
            data.npcPositions.Find(n =>
                n.npcID == npcID &&
                n.sceneName == currentScene
            );

        if (npc != null)
        {
            return npc.GetPosition();
        }

        return Vector3.zero;
    }

    // =========================
    // NPC DIALOGUE STATE
    // =========================

    public int GetNPCDialogueState(string npcID)
    {
        if (data == null) return 0;

        NPCDialogueState state =
            data.npcDialogueStates.Find(n => n.npcID == npcID);

        if (state != null)
            return state.dialogueIndex;

        return 0;
    }

    public void SetNPCDialogueState(string npcID, int index)
    {
        if (data == null) data = new GameData();

        NPCDialogueState state =
            data.npcDialogueStates.Find(n => n.npcID == npcID);

        if (state != null)
        {
            state.dialogueIndex = index;
        }
        else
        {
            data.npcDialogueStates.Add(
                new NPCDialogueState(npcID, index)
            );
        }

        SaveGame();
    }

    // =========================
    // AUTO SAVE
    // =========================
    private void OnApplicationQuit()
    {
        Debug.Log("[GameManager] Menyimpan data sebelum keluar game...");
        SaveGame();
    }

    // Fungsi untuk mengecek apakah item sudah pernah diambil sebelumnya
    public bool IsItemCollected(string uniqueID)
    {
        if (data != null && data.collectedItems != null)
        {
            return data.collectedItems.Contains(uniqueID);
        }
        return false;
    }

    // Fungsi untuk mencatat item yang baru saja diambil
    public void MarkItemAsCollected(string uniqueID)
    {
        if (data != null)
        {
            if (data.collectedItems == null)
            {
                data.collectedItems = new List<string>();
            }

            // Jika belum ada di catatan, masukkan ke catatan
            if (!data.collectedItems.Contains(uniqueID))
            {
                data.collectedItems.Add(uniqueID);
            }
        }
    }
}