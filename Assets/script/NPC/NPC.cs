using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    public enum InteractionType
    {
        Auto,
        PressKey
    }

    [Header("NPC ID")]
    public string npcID;

    [Header("UI")]
    public Image alertPanel;

    [Header("Dialogue")]
    public DialogueSequence dialogueData;

    [Header("Next Dialogue")]
    public NPC nextNPC;

    [Header("After Dialogue")]
    public bool moveAfterDialogue;

    [Header("Optional Scene")]
    public string targetScene;

    [Header("Interaction Type")]
    public InteractionType interactionType = InteractionType.PressKey;

    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Settings")]
    public bool oneTimeOnly = false;

    private bool playerInRange;
    private bool hasTriggered;

    // =========================
    // SAVE SYSTEM OPTIONS
    // =========================
    [Header("Save System")]
    public bool saveAfterDialogue = false;
    public bool markNpcMet = true;
    public bool markNpcGone = false;

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
        // =========================
        // MOVE AFTER DIALOGUE
        // =========================
        if (moveAfterDialogue)
        {
            NPCMover mover = GetComponent<NPCMover>();

            if (mover != null)
                mover.StartMove();
        }

        // =========================
        // NEXT NPC DIALOGUE
        // =========================
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