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
        if (playerPrefab == null) return;

        var data = GameManager.Instance.data;
        if (data == null) return;

        string currentScene = SceneManager.GetActiveScene().name;

        // Cari data posisi spesifik untuk scene ini
        ScenePosition pos = data.scenePositions.Find(s => s.sceneName == currentScene);
        Vector3 spawnPosition;

        if (pos != null)
        {
            // Pakai koordinat simpanan jika ada
            spawnPosition = new Vector3(pos.x, pos.y, pos.z);
        }
        else
        {
            // Pakai koordinat default tempat PlayerSpawnManager ditaruh di Unity Editor
            spawnPosition = transform.position;
        }

        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}