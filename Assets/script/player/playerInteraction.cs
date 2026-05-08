using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject pressEText;

    [Header("Auto Start Dialogue")]
    public NPC startingNPC;

    private NPC currentNPC;

    private void Start()
    {

        if (!dialogueManager.IsPlaying() && currentNPC != null)
        {
            pressEText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueManager.StartDialogue(currentNPC.GetCurrentDialogue(), currentNPC);
            }
        }
        pressEText.SetActive(false);
        StartCoroutine(StartDialogueDelay());
    }

    IEnumerator StartDialogueDelay()
    {
        yield return new WaitForSeconds(0.2f);

        if (startingNPC == null || dialogueManager == null) yield break;

        currentNPC = startingNPC;
        dialogueManager.StartDialogue(currentNPC.GetCurrentDialogue(), currentNPC);
    }

    void Update()
    {
        if (dialogueManager.IsPlaying())
        {
            pressEText.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueManager.NextLine();
            }

            return;
        }

        if (currentNPC != null)
        {
            pressEText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                dialogueManager.StartDialogue(currentNPC.GetCurrentDialogue(), currentNPC);
            }
        }
        else
        {
            pressEText.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc != null) currentNPC = npc;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        NPC npc = other.GetComponent<NPC>();
        if (npc != null && npc == currentNPC) currentNPC = null;
    }
}