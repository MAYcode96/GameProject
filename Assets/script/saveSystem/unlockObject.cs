using UnityEngine;

public class UnlockObject : MonoBehaviour
{
    public string objectID;
    public bool saveAfterUnlock = true;

    public void Unlock()
    {
        if (GameManager.Instance == null) return;
        if (string.IsNullOrEmpty(objectID)) objectID = gameObject.name;

        // Tinggal panggil fungsi tiruan yang ada di GameManager
        GameManager.Instance.SetObjectUnlocked(objectID, saveAfterUnlock);
    }
}