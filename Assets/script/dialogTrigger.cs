using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSequence dialogue;

    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }

        if (DialogueManager.Instance.IsPlaying() && Input.GetKeyDown(KeyCode.Space))
        {
            DialogueManager.Instance.NextLine();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}