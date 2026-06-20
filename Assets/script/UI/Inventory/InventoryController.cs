using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        // Cek apakah slot sudah ada. Jika belum, buat slot sejumlah slotCount
        if (inventoryPanel.transform.childCount == 0)
        {
            for (int i = 0; i < slotCount; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab, inventoryPanel.transform);
                // Tambahan: Pastikan slot memiliki script Slot
                if (newSlot.GetComponent<Slot>() == null)
                {
                    Debug.LogError("Prefab slot kamu tidak memiliki script Slot!");
                }
            }
        }
        
        // Inisialisasi dictionary
        if (itemDictionary == null)
        {
            itemDictionary = FindFirstObjectByType<ItemDictionary>(FindObjectsInactive.Include);
            if (itemDictionary != null) itemDictionary.InitDictionary();
        }
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
                }
            }
        }

        // TAMBAHKAN INI UNTUK MELACAK DATA
        Debug.Log($"[Save] Berhasil mendata {invData.Count} item dari UI Inventory untuk disimpan.");
        return invData;
    }
    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        if (inventorySaveData == null) return;

        if (itemDictionary == null)
        {
            itemDictionary = FindFirstObjectByType<ItemDictionary>(FindObjectsInactive.Include);
            // Pastikan dictionary diinisialisasi (memanggil fungsi yang kita buat di langkah sebelumnya)
            if (itemDictionary != null) itemDictionary.InitDictionary();
        }

        Debug.Log($"[Load] Mulai memuat {inventorySaveData.Count} item ke inventory.");

        // 1. Pastikan jumlah slot sesuai dengan slotCount. Jika belum ada, buat.
        while (inventoryPanel.transform.childCount < slotCount)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        // 2. Bersihkan HANYA item yang ada di dalam slot (kosongkan inventory)
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }
        }

        // 3. Masukkan item dari data save ke slot yang tepat
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);

                    RectTransform rect = item.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        rect.anchorMin = new Vector2(0.5f, 0.5f);
                        rect.anchorMax = new Vector2(0.5f, 0.5f);
                        rect.pivot = new Vector2(0.5f, 0.5f);
                        rect.anchoredPosition = Vector2.zero;
                    }

                    slot.currentItem = item;
                }
                else
                {
                    Debug.LogError($"[Load] Gagal memuat item! Prefab dengan ID {data.itemID} tidak ditemukan.");
                }
            }
        }
    }

    // Fungsi untuk menambahkan item lewat script/gameplay
    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            // Cari slot yang kosong
            if (slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);

                // Reset posisi
                RectTransform rect = newItem.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchorMin = new Vector2(0.5f, 0.5f);
                    rect.anchorMax = new Vector2(0.5f, 0.5f);
                    rect.pivot = new Vector2(0.5f, 0.5f);
                    rect.anchoredPosition = Vector2.zero;
                }

                slot.currentItem = newItem;

                // PASTIKAN SAVE DIPANGGIL SETELAH ITEM DITAMBAHKAN
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SaveGame();
                }

                return true; // Berhasil masuk inventory
            }
        }

        Debug.Log("Inventory Penuh!");
        return false; // Inventory penuh
    }

    // Fungsi baru untuk memasukkan item ke slot kosong berdasarkan ID-nya saja
    public bool AddItemByID(int itemID)
    {
        Debug.Log("=== CEK INVENTORY ===");
        Debug.Log("InventoryController = " + gameObject.name);
        Debug.Log("InventoryPanel = " + inventoryPanel.name);
        Debug.Log("ChildCount = " + inventoryPanel.transform.childCount);

        int slotTerisi = 0;

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null)
            {
                Debug.Log(
                    $"Slot {slotTransform.GetSiblingIndex()} : " +
                    (slot.currentItem == null ? "KOSONG" : slot.currentItem.name)
                );

                if (slot.currentItem != null)
                    slotTerisi++;
            }
        }

        Debug.Log($"Slot terisi: {slotTerisi}/{inventoryPanel.transform.childCount}");
        if (itemDictionary == null)
        {
            itemDictionary = FindFirstObjectByType<ItemDictionary>(FindObjectsInactive.Include);
            if (itemDictionary != null) itemDictionary.InitDictionary();
        }

        // Ambil UI prefab item dari dictionary berdasarkan ID
        GameObject itemPrefab = itemDictionary.GetItemPrefab(itemID);
        if (itemPrefab == null) return false;

        // Cari slot yang kosong di UI panel
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                // Instantiate item masuk ke dalam slot UI
                GameObject newItem = Instantiate(itemPrefab, slot.transform);

                RectTransform rect = newItem.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchorMin = new Vector2(0.5f, 0.5f);
                    rect.anchorMax = new Vector2(0.5f, 0.5f);
                    rect.pivot = new Vector2(0.5f, 0.5f);
                    rect.anchoredPosition = Vector2.zero;
                }

                slot.currentItem = newItem;

                // Auto-save setelah berhasil mengambil barang
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SaveGame();
                }

                return true; // Berhasil masuk inventory
            }
        }

        Debug.LogWarning("Inventory Penuh! Tidak bisa mengambil item.");
        return false; // Gagal karena inventory penuh
    }

}