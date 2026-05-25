using UnityEngine;

public class UnlockObject : MonoBehaviour
{
    public string objectID;
    public bool saveAfterUnlock = true;

    public void Unlock()
    {
        Debug.Log("UNLOCK DIPANGGIL");

        if (GameManager.Instance == null) return;

        if (string.IsNullOrEmpty(objectID))
            objectID = gameObject.name;

        GameManager.Instance.SetObjectUnlocked(objectID, saveAfterUnlock);
    }
}