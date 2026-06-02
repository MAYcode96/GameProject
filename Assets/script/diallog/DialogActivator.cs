using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    [Header("Data Dialog")]
    public DialogueSequence dialogueData;

    private bool playerNearby;

    void Update()
    {
        // Tombol E ditekan dan player ada di dekatnya
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Memanggil fungsi StartDialogue dengan parameter baru
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}