using UnityEngine;

public class UnlockObject : MonoBehaviour
{
    [Header("Semua ID yang mau di-unlock")]
    public string[] objectIDs;

    public bool saveAfterUnlock = true;

    public void Unlock()
    {
        Debug.Log("MULTI UNLOCK DIPANGGIL");

        if (GameManager.Instance == null) return;

        foreach (string id in objectIDs)
        {
            if (string.IsNullOrEmpty(id)) continue;

            GameManager.Instance.SetObjectUnlocked(id, saveAfterUnlock);

            Debug.Log("Unlocked ID: " + id);
        }
    }
}