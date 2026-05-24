using UnityEngine;

public class SetNPCGone : MonoBehaviour
{
    public string npcID;

    // Panggil fungsi ini saat dialog selesai, quest beres, atau saat NPC harus pergi
    public void RemoveNPC()
    {
        if (GameManager.Instance != null)
        {
            // GameManager akan otomatis mengurus pengecekan data, penguncian posisi player, dan autosave
            GameManager.Instance.SetNpcGone(npcID);
        }
    }
}