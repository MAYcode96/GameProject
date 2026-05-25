using UnityEngine;
using System.Collections;

public class ObjectUnlockChecker : MonoBehaviour
{
    public string objectID;
    public GameObject targetObject;

    IEnumerator Start()
    {
        // Tunggu sampai GameManager dan datanya benar-benar siap
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

        // Cek ID 
        bool unlocked = GameManager.Instance.IsObjectUnlocked(objectID);
        targetObject.SetActive(unlocked);

        Debug.Log(unlocked
            ? $"[Checker] {objectID} ADA di save -> Objek AKTIF"
            : $"[Checker] {objectID} TIDAK ADA di save -> Objek NON-AKTIF");
    }
}