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
        pressEText.SetActive(false);
        StartCoroutine(StartDialogueDelay());
    }

    IEnumerator StartDialogueDelay()
    {
        yield return new WaitForSeconds(0.2f);

        if (startingNPC == null)
        {
            Debug.LogError("Starting NPC belum di-assign!");
            yield break;
        }

        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager belum di-assign!");
            yield break;
        }

        if (startingNPC.dialogue == null)
        {
            Debug.LogError("Dialogue di NPC kosong!");
            yield break;
        }

        currentNPC = startingNPC;
        dialogueManager.StartDialogue(currentNPC.dialogue);
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
                dialogueManager.StartDialogue(currentNPC.dialogue);
                pressEText.SetActive(false);
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

        if (npc != null)
        {
            currentNPC = npc;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        NPC npc = other.GetComponent<NPC>();

        if (npc != null && npc == currentNPC)
        {
            currentNPC = null;
        }
    }
}