using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public enum InteractionType
    {
        Auto,
        PressKey
    }

    public Image alertPanel;
    public DialogueSequence dialogueData;
    public NPC nextNPC;
    public bool moveAfterDialogue;
    public string targetScene;
    public InteractionType interactionType = InteractionType.PressKey;
    public KeyCode interactKey = KeyCode.E;
    public bool oneTimeOnly = false;

    [Header("Events")] 
    public UnityEvent onDialogueEnd;
    private bool playerInRange;
    private bool hasTriggered;

    void Start()
    {
        if (alertPanel != null)
            alertPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (interactionType != InteractionType.PressKey)
            return;

        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.isDialogueOpen)
            return;

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            StartNPCDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("player"))
            return;

        playerInRange = true;

        if (interactionType == InteractionType.Auto)
        {
            if (alertPanel != null)
                alertPanel.gameObject.SetActive(false);

            StartNPCDialogue();
            return;
        }

        if (alertPanel != null)
            alertPanel.gameObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("player"))
            return;

        playerInRange = false;

        if (alertPanel != null)
            alertPanel.gameObject.SetActive(false);
    }

    public void StartNPCDialogue()
    {
        if (DialogueManager.Instance == null)
            return;

        if (dialogueData == null)
            return;

        if (oneTimeOnly && hasTriggered)
            return;

        if (DialogueManager.Instance.gameObject == null)
            return;

        hasTriggered = true;

        DialogueManager.Instance.StartDialogue(
            dialogueData,
            targetScene,
            this
        );
    }
    

    public void StartForcedDialogue()
    {
        StartNPCDialogue();
    }

    public void OnDialogueFinished()
    {
        if (onDialogueEnd != null)
        {
            onDialogueEnd.Invoke();
        }

        // MOVE AFTER DIALOGUE
        if (moveAfterDialogue)
        {
            NPCMover mover = GetComponent<NPCMover>();

            if (mover != null)
                mover.StartMove();
        }
    
        // NEXT NPC DIALOGUE
        if (nextNPC != null)
        {
            StartCoroutine(StartNextNPCDialogue());
        }
    }

    IEnumerator StartNextNPCDialogue()
    {
        yield return new WaitForSeconds(0.7f);

        if (nextNPC != null)
            nextNPC.StartForcedDialogue();
    }
}