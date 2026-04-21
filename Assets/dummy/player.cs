using UnityEngine;

public class PlayerDrag2D : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 startPosition;
    private Transform startTile;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        offset = transform.position - mousePos;

        startPosition = transform.position;
        startTile = transform.parent;
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        transform.position = mousePos + offset;
    }

    void OnMouseUp()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position);

        if (hit != null && hit.CompareTag("tile"))
        {
            Transform targetTile = hit.transform;

            if (targetTile.childCount == 0)
            {
                transform.position = targetTile.position;
                transform.parent = targetTile;
                return;
            }
        }

        transform.position = startPosition;
        transform.parent = startTile;
    }
}