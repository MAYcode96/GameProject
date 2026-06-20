using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float followSmoothTime = 0.2f; 

    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (target == null)
        {
            FindPlayerTarget();
        }
    }

    void LateUpdate()
{
 
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

        float clampedX = Mathf.Clamp(smoothPos.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        float clampedY = Mathf.Clamp(smoothPos.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}