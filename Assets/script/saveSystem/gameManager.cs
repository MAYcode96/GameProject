using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData data;

    private Transform playerCache;

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

    // =========================
    // PLAYER CACHE
    // =========================
    public void RegisterPlayer(Transform player)
    {
        playerCache = player;
    }

    // =========================
    // OBJECT SYSTEM
    // =========================
    public void MarkObjectUnlocked(string id)
    {
        if (data == null) data = new GameData();
        if (!data.objectUnlocked.Contains(id))
            data.objectUnlocked.Add(id);
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

    public bool IsObjectUnlocked(string id)
    {
        return data != null && data.objectUnlocked.Contains(id);
    }

    // =========================
    // NPC SYSTEM
    // =========================
    public void MarkNpcGone(string id)
    {
        if (data == null) data = new GameData();
        if (!data.npcGone.Contains(id))
        {
            data.npcGone.Add(id);
            LockCurrentPlayerState();
        }
    }

    public void SetNpcGone(string id) => MarkNpcGone(id);

    public void SetNpcMet(string id)
    {
        if (data == null) data = new GameData();
        if (!data.npcMet.Contains(id))
        {
            data.npcMet.Add(id);
            LockCurrentPlayerState();
        }
    }

    public bool IsNpcGone(string id) => data != null && data.npcGone.Contains(id);
    public bool IsNpcMet(string id) => data != null && data.npcMet.Contains(id);

    // =========================
    // ITEM SYSTEM
    // =========================
    public bool IsItemCollected(string id)
    {
        if (data != null && data.collectedItems != null)
            return data.collectedItems.Contains(id);
        return false;
    }

    public void MarkItemAsCollected(string id)
    {
        if (data == null) data = new GameData();
        if (data.collectedItems == null)
            data.collectedItems = new List<string>();
        
        if (!data.collectedItems.Contains(id))
            data.collectedItems.Add(id);
    }

    // =========================
    // SAVE SYSTEM
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
        InventoryController inventoryController = FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);
        if (inventoryController != null)
        {
            data.inventorySaveData = inventoryController.GetInventoryItems();
        }

        SaveSystem.Save(data);
        Debug.Log("[GameManager] SAVE COMPLETE");
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
    // SCENE CHANGE
    // =========================
    public void ChangeScene(string sceneName)
    {
        SavePlayerPosition();       // simpan posisi player
        SaveGame();                 // save 1x saja

        SceneManager.LoadScene(sceneName);
    }

    // =========================
    // PLAYER POSITION SAVE
    // =========================
    public void SavePlayerPosition()
    {
        if (playerCache == null) return;
        SavePlayerPositionInternal();
    }

    public void SavePlayerPosition(Transform playerTransform, bool forceWriteToDisk)
    {
        if (playerTransform == null) return;
        playerCache = playerTransform;
        SavePlayerPositionInternal();

        if (forceWriteToDisk)
            SaveGame();
    }

    private void SavePlayerPositionInternal()
    {
        if (data == null) data = new GameData();

        string scene = SceneManager.GetActiveScene().name;
        if (scene == "MainMenu") return;

        data.lastScene = scene;

        ScenePosition existing = data.scenePositions.Find(s => s.sceneName == scene);

        if (existing != null)
        {
            existing.x = playerCache.position.x;
            existing.y = playerCache.position.y;
            existing.z = playerCache.position.z;
        }
        else
        {
            data.scenePositions.Add(
                new ScenePosition(scene, playerCache.position)
            );
        }
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
    // APPLY STATE AFTER LOAD
    // =========================
    public void ApplyObjectStates()
    {
        if (data == null) return;

        ObjectUnlockChecker[] checkers =
            Object.FindObjectsByType<ObjectUnlockChecker>(FindObjectsSortMode.None);

        foreach (var checker in checkers)
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

        Debug.Log("[GameManager] OBJECT STATES APPLIED / SEMUA STATUS CHECKER TELAH DIPERBARUI");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyObjectStates();
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    // =========================
    // AUTO SAVE ON QUIT
    // =========================
    private void OnApplicationQuit()
    {
        Debug.Log("[GameManager] Menyimpan data sebelum keluar game...");
        SaveGame();
    }

    // =========================
    // CONTINUE GAME
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
}