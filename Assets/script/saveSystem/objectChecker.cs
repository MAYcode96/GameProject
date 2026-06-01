using UnityEngine;
using System.Collections;

public class ObjectUnlockChecker : MonoBehaviour
{
    public string objectID;

    [Header("Optional NPC Gone ID")]
    public string npcGoneID;

    public GameObject[] targetObjects;

    IEnumerator Start()
    {
        while (GameManager.Instance == null || GameManager.Instance.data == null)
        {
            yield return null;
        }

        if (string.IsNullOrEmpty(objectID))
        {
            objectID = gameObject.name;
        }

        EvaluateState();
    }

    public void EvaluateState()
    {
        if (GameManager.Instance == null) return;

        bool unlocked =
            GameManager.Instance.IsObjectUnlocked(objectID);

        bool npcGone =
            !string.IsNullOrEmpty(npcGoneID) &&
            GameManager.Instance.IsNpcGone(npcGoneID);

        // PRIORITAS:
        // kalau NPC gone -> paksa hilang
        if (npcGone)
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            Debug.Log($"[Checker] NPC GONE: {npcGoneID}");
            return;
        }

        // kalau belum gone -> cek unlock
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
                obj.SetActive(unlocked);
        }

        Debug.Log(unlocked
            ? $"[Checker] {objectID} AKTIF"
            : $"[Checker] {objectID} NONAKTIF");
    }
}