using UnityEngine;

public class NPCGoneChecker : MonoBehaviour
{
    public string npcID;

    void Start()
    {
        // Menghubungi GameManager untuk cek apakah ID NPC ini sudah masuk daftar 'hilang'
        if (GameManager.Instance != null && GameManager.Instance.IsNpcGone(npcID))
        {
            gameObject.SetActive(false);

            // Kalau mau benar-benar dihapus dari memori scene:
            // Destroy(gameObject);
        }
    }
}