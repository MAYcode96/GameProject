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

        // Cek apakah target sudah diisi di Inspector
        if (target == null)
        {
            Debug.LogError("Waduh! Target di CameraController belum diisi. Tarik objek Player ke kolom Target!");
        }
    }

    void LateUpdate()
    {
        // Jika target ada, langsung jalankan follow (tanpa perlu trigger lagi)
        if (target != null)
        {
            HandleFollow();
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

        // CLAMPING
        float clampedX = Mathf.Clamp(smoothPos.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        float clampedY = Mathf.Clamp(smoothPos.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}