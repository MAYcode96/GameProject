using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("NPC ID (WAJIB UNIK)")]
    public string npcID;

    [Header("NPC Settings")]
    public bool disappearWhenGone = true;

    void Start()
    {
        if (string.IsNullOrEmpty(npcID))
        {
            Debug.LogError("NPC ID belum diisi: " + gameObject.name);
            return;
        }

        if (GameManager.Instance.IsNpcGone(npcID))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTalk()
    {
        if (GameManager.Instance.IsNpcGone(npcID))
            return;

        GameManager.Instance.SetNpcMet(npcID);
        GameManager.Instance.SaveGame();
    }

    public void MakeDisappear()
    {
        GameManager.Instance.SetNpcGone(npcID);
        GameManager.Instance.SaveGame();

        if (disappearWhenGone)
            gameObject.SetActive(false);
    }
}