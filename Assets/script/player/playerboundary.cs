using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    // Kita ubah jadi private/hide karena akan dicari otomatis oleh script
    private Camera cam;
    private float halfWidth;

    void Start()
    {
        // 1. Cari Main Camera secara otomatis di dalam scene
        if (cam == null)
        {
            cam = Camera.main;

            // Pengaman jika Camera.main tidak ketemu atau lupa diset Tag-nya di Unity
            if (cam == null)
            {
                cam = FindFirstObjectByType<Camera>();
            }

            if (cam == null)
            {
                Debug.LogError("PlayerBoundary: Waduh, Kamera tidak ditemukan di dalam scene!");
            }
        }

        // 2. Ambil setengah lebar player dari SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            halfWidth = spriteRenderer.bounds.extents.x;
        }
        else
        {
            Debug.LogWarning("PlayerBoundary: Objek tidak memiliki SpriteRenderer!");
            halfWidth = 0f;
        }
    }

    void LateUpdate()
    {
        // Jika kamera belum ditemukan, jangan jalankan pembatasan dulu
        if (cam == null) return;

        // Batas bawah kiri dan atas kanan kamera berdasarkan viewport
        Vector3 leftBound = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightBound = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));

        Vector3 pos = transform.position;

        // Batasi posisi X player agar tidak keluar dari pandangan kamera kiri & kanan
        pos.x = Mathf.Clamp(
            pos.x,
            leftBound.x + halfWidth,
            rightBound.x - halfWidth
        );

        transform.position = pos;
    }
}