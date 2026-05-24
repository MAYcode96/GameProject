using UnityEngine;

public class ObjectUnlockChecker : MonoBehaviour
{
    [Header("Object ID")]
    public string objectID;

    [Header("Target Object to Activate")]
    public GameObject targetObject;

    void Start()
    {
        // Pengaman: Jika targetObject tidak diisi, gunakan objek ini sendiri
        if (targetObject == null)
        {
            targetObject = this.gameObject;
        }

        // Validasi ID Otomatis: Harus sama polanya dengan UnlockObject
        if (string.IsNullOrEmpty(objectID))
        {
            objectID = gameObject.name;
        }

        // Matikan objek terlebih dahulu secara default
        targetObject.SetActive(false);

        // Cek ke data GameManager apakah ID ini sudah pernah di-unlock
        if (GameManager.Instance != null && GameManager.Instance.data != null)
        {
            if (GameManager.Instance.data.objectUnlocked.Contains(objectID))
            {
                targetObject.SetActive(true);
                Debug.Log($"[UnlockChecker] {objectID} aktif karena sudah di-unlock sebelumnya.");
            }
        }
    }
}