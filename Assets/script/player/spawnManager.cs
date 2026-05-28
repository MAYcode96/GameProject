using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;
    public GameObject playerPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab belum diisi!");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager tidak ditemukan!");
            return;
        }

        if (GameManager.Instance.data == null)
        {
            Debug.LogError("GameData belum ada!");
            return;
        }

        var data = GameManager.Instance.data;

        string currentScene = SceneManager.GetActiveScene().name;

        ScenePosition pos = data.scenePositions.Find(s => s.sceneName == currentScene);

        Vector3 spawnPosition;

        if (pos != null)
        {
            spawnPosition = new Vector3(pos.x, pos.y, pos.z);
        }
        else
        {
            spawnPosition = transform.position;
        }

        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}