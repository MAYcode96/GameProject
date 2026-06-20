using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDrag2D : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 startPosition;
    private Transform startTile;

    private Camera cam;

    [Header("Snap Settings")]
    public float snapDistance = 1f;

    [HideInInspector]
    public bool isOnTile = true;

    [HideInInspector]
    public bool canDrag = true;

    void Start()
    {
        cam = Camera.main;

        Debug.Log("=== PLAYER DEBUG ===");

        if (cam == null)
            Debug.LogError("MainCamera tidak ditemukan!");
        else
            Debug.Log("MainCamera ditemukan");

        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
            Debug.LogError("Collider2D TIDAK ADA!");
        else
            Debug.Log("Collider2D ditemukan: " + col.GetType().Name);
    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown terpanggil");

        if (!canDrag)
        {
            Debug.Log("canDrag = false");
            return;
        }

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Klik berada di atas UI");
            return;
        }

        Debug.Log("Mulai Drag");

        isOnTile = false;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        offset = transform.position - mousePos;

        startPosition = transform.position;
        startTile = transform.parent;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Yang kena klik: " + hit.collider.name);
            }
            else
            {
                Debug.Log("Tidak kena apa-apa");
            }
        }
    }
    void OnMouseDrag()
    {
        if (!canDrag)
            return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        transform.position = mousePos + offset;
    }

    void OnMouseUp()
    {
        if (!canDrag)
            return;

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");

        Transform nearestTile = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject tile in tiles)
        {
            float distance = Vector2.Distance(
                transform.position,
                tile.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTile = tile.transform;
            }
        }

        if (nearestTile != null && nearestDistance <= snapDistance)
        {
            bool occupied = false;

            foreach (Transform child in nearestTile)
            {
                if (child != transform)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                transform.SetParent(null);
                transform.position = nearestTile.position;
                transform.SetParent(nearestTile);
                isOnTile = true;

                Debug.Log("Snap berhasil");
            }
            else
            {
                transform.position = startPosition;
                transform.SetParent(startTile);
                isOnTile = true;

                Debug.Log("Tile sudah terisi");
            }
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startTile);
            isOnTile = true;

            Debug.Log("Tidak ada tile valid");
        }
    }
}