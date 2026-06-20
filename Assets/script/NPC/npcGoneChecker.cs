using UnityEngine;

public class NPCGoneChecker : MonoBehaviour
{
    public string npcID;

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsNpcGone(npcID))
        {
            gameObject.SetActive(false);

        }
    }
}