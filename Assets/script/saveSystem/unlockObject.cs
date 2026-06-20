using UnityEngine;

public class UnlockObject : MonoBehaviour
{
    public string[] objectIDs;

    public void Unlock()
    {
        if (GameManager.Instance == null) return;

        foreach (string id in objectIDs)
        {
            if (string.IsNullOrEmpty(id)) continue;

            GameManager.Instance.MarkObjectUnlocked(id);
        }

        Debug.Log("Unlock batch selesai");
    }
}