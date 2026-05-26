using UnityEngine;

public class SetNPCGone : MonoBehaviour
{
    [Header("Semua NPC yang mau dihilangkan")]
    public string[] npcIDs;

    // Panggil saat dialog selesai / quest selesai
    public void RemoveNPC()
    {
        if (GameManager.Instance == null) return;

        foreach (string id in npcIDs)
        {
            if (string.IsNullOrEmpty(id)) continue;

            GameManager.Instance.SetNpcGone(id);

            Debug.Log("NPC Gone: " + id);
        }
    }
}