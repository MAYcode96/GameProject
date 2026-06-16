using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDict; // Ubah nama variabel agar tidak bingung dengan nama Class
    private bool isInitialized = false;

    private void Awake()
    {
        InitDictionary();
    }

    // Buat fungsi khusus untuk inisialisasi
    public void InitDictionary()
    {
        if (isInitialized) return; // Cegah inisialisasi ganda

        itemDict = new Dictionary<int, GameObject>();

        // Auto Increment Ids
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }
        }

        foreach (Item item in itemPrefabs)
        {
            if (item != null && !itemDict.ContainsKey(item.ID))
            {
                itemDict.Add(item.ID, item.gameObject);
            }
        }

        isInitialized = true;
    }

    public GameObject GetItemPrefab(int itemID)
    {
        // Pastikan dictionary sudah siap sebelum dicari
        InitDictionary(); 

        itemDict.TryGetValue(itemID, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"Item dengan ID {itemID} tidak ditemukan di dictionary");
        }
        return prefab;
    }
}