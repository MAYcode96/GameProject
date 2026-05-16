using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    public Camera cam;

    private float halfWidth;

    void Start()
    {
        // Ambil setengah lebar player
        halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void LateUpdate()
    {
        // Batas bawah kiri dan atas kanan kamera
        Vector3 leftBound = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightBound = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));

        Vector3 pos = transform.position;

        // Batasi posisi X player
        pos.x = Mathf.Clamp(
            pos.x,
            leftBound.x + halfWidth,
            rightBound.x - halfWidth
        );

        transform.position = pos;
    }
}