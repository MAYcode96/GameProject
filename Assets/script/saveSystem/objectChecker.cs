using UnityEngine;
using System.Collections;

public class ObjectUnlockChecker : MonoBehaviour
{
    public string objectID;

    [Header("Optional NPC Gone ID")]
    public string npcGoneID;

    public GameObject targetObject;

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

        if (targetObject == null)
        {
            targetObject = this.gameObject;
        }

        EvaluateState();
    }

    public void EvaluateState()
    {
        if (GameManager.Instance == null || targetObject == null) return;

        bool unlocked =
            GameManager.Instance.IsObjectUnlocked(objectID);

        bool npcGone =
            !string.IsNullOrEmpty(npcGoneID) &&
            GameManager.Instance.IsNpcGone(npcGoneID);

        // PRIORITAS:
        // kalau NPC gone -> paksa hilang
        if (npcGone)
        {
            targetObject.SetActive(false);

            Debug.Log($"[Checker] NPC GONE: {npcGoneID}");
            return;
        }

        // kalau belum gone -> cek unlock
        targetObject.SetActive(unlocked);

        Debug.Log(unlocked
            ? $"[Checker] {objectID} AKTIF"
            : $"[Checker] {objectID} NONAKTIF");
    }
}