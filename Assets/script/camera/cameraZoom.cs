using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float followSmoothTime = 0.2f; // Pastikan ini POSITIF (0.2 atau 0.3)

    [Header("Level Boundaries")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Cari target secara otomatis jika di awal game masih kosong
        if (target == null)
        {
            FindPlayerTarget();
        }
    }

    void LateUpdate()
{
    // Jika karena suatu hal target hilang atau belum ketemu (misal saat transisi spawn), 
        // kamera akan terus mencoba mencari sampai ketemu
    if (Time.timeScale == 0f)
    {
        velocity = Vector3.zero;
        return;
    }

    if (target == null)
    {
        FindPlayerTarget();
        return;
    }

    HandleFollow();
}

    void FindPlayerTarget()
    {
        // Mencari objek di scene yang memiliki tag "player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("player");
        if (playerObj != null)
        {
            target = playerObj.transform;
            Debug.Log("CameraController: Target Player berhasil ditemukan otomatis!");
        }
    }

    void HandleFollow()
    {
        Vector3 targetPos = target.position + offset;

        Vector3 smoothPos = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            followSmoothTime
        );

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // CLAMPING (Agar kamera tidak keluar dari batas map yang kamu tentukan)
        float clampedX = Mathf.Clamp(smoothPos.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        float clampedY = Mathf.Clamp(smoothPos.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}