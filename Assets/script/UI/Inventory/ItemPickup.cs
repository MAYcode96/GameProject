using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public int itemID; // ID untuk jenis item di inventory
    
    // TAMBAHKAN INI: ID unik khusus untuk objek ini di dalam scene
    [Tooltip("Isi dengan nama unik, contoh: gelang_gerbang_01")]
    public string uniqueSceneID; 

    [Header("UI Prompt Settings")]
    public GameObject interactionPrompt;

    private bool isPlayerInRange = false;
    private InventoryController inventoryController;

    void Start()
    {
        // 1. CEK STATUS: Apakah item ini sudah pernah diambil?
        if (GameManager.Instance != null && GameManager.Instance.IsItemCollected(uniqueSceneID))
        {
            // PERBAIKAN: Matikan dulu tulisan E sebelum objek dihancurkan!
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            Destroy(gameObject);
            return; // Hentikan eksekusi
        }

        // 2. Jika belum diambil, jalankan fungsi normal (sembunyikan prompt di awal)
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        inventoryController = FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
    }

    void PickUpItem()
    {
        if (inventoryController == null)
        {
            inventoryController = FindFirstObjectByType<InventoryController>(FindObjectsInactive.Include);
        }

        if (inventoryController != null)
        {
            bool isPickedUp = inventoryController.AddItemByID(itemID);

            if (isPickedUp)
            {
                // PENTING: Catat di GameManager bahwa item ini sudah diambil
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.MarkItemAsCollected(uniqueSceneID);
                    GameManager.Instance.SaveGame(); // Save data terbaru
                }

                Debug.Log($"[Pickup] Berhasil mengambil item {uniqueSceneID}");
                Destroy(gameObject);
            }
        }
    }

    // ... (Bagian OnTriggerEnter2D dan OnTriggerExit2D tetap sama seperti sebelumnya) ..
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null) interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerInRange = false;
            if (interactionPrompt != null) interactionPrompt.SetActive(false);
        }
    }
}