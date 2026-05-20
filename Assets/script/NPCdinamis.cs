using UnityEngine;

public class DynamicNPC : MonoBehaviour
{
    public DialogueSequence defaultDialogue;

    public DialogueSequence completedDialogue;

    public bool isQuestCompleted;

    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!DialogueManager.Instance.isDialogueOpen)
            {
                if (isQuestCompleted)
                {
                    DialogueManager.Instance.StartDialogue(completedDialogue);
                }
                else
                {
                    DialogueManager.Instance.StartDialogue(defaultDialogue);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}