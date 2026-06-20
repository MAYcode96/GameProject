using UnityEngine;

public class SetNPCGone : MonoBehaviour
{
    public string[] npcIDs;

    public void RemoveNPC()
    {
        if (GameManager.Instance == null) return;

        foreach (string id in npcIDs)
        {
            if (string.IsNullOrEmpty(id)) continue;

            GameManager.Instance.MarkNpcGone(id);
        }

        Debug.Log("NPC batch selesai");
    }
}