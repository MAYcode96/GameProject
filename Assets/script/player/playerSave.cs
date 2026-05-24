using UnityEngine;
using System.Collections;

public class AutoSavePlayer : MonoBehaviour
{
    [Header("Interval Settings")]
    [Tooltip("Mengirim koordinat ke ram GameManager setiap x detik")]
    public float memoryUpdateInterval = 1f;

    [Tooltip("Menulis data ram GameManager ke file JSON fisik setiap x detik")]
    public float diskWriteInterval = 5f;

    private float diskTimer;

    void Start()
    {
        diskTimer = diskWriteInterval;

        // Beri Tag objek ini secara otomatis saat runtime untuk mencegah kelalaian manusia
        gameObject.tag = "player";

        StartCoroutine(AutoSaveRoutine());
    }

    IEnumerator AutoSaveRoutine()
    {
        // JEDA 1 DETIK UTAMA: Menahan detektor agar tidak langsung menimpa data 
        // saat PlayerSpawnManager baru saja memproses kelahiran player.
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            if (GameManager.Instance != null)
            {
                diskTimer -= memoryUpdateInterval;

                if (diskTimer <= 0)
                {
                    // Waktunya menulis file fisik (.json) ke storage
                    GameManager.Instance.SavePlayerPosition(transform, true);
                    diskTimer = diskWriteInterval; // Reset timer disk
                    Debug.Log($"<color=white>[AutoSave] File JSON Diperbarui Disk! Posisi: {transform.position}</color>");
                }
                else
                {
                    // Update data di memori internal GameManager saja (Ringan & Aman)
                    GameManager.Instance.SavePlayerPosition(transform, false);
                    Debug.Log($"<color=orange>[AutoSave] Memori RAM Sinkron: {transform.position}</color>");
                }
            }

            yield return new WaitForSeconds(memoryUpdateInterval);
        }
    }
}