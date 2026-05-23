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

    [Header("UI")]
    public Image alertPanel;

    [Header("Dialogue")]
    public DialogueSequence dialogueData;

    [Header("Next Dialogue")]
    public NPC nextNPC;

    [Header("After Dialogue")]
    public bool moveAfterDialogue;

    [Header("Optional")]
    public string targetScene;

    [Header("Interaction Type")]
    public InteractionType interactionType = InteractionType.PressKey;

    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Settings")]
    public bool oneTimeOnly = false;

    private bool playerInRange;
    private bool hasTriggered;

    void Start()
    {
        if (alertPanel != null)
        {
            alertPanel.gameObject.SetActive(false);
        }
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
            {
                alertPanel.gameObject.SetActive(false);
            }

            StartNPCDialogue();

            return;
        }

        if (alertPanel != null)
        {
            alertPanel.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("player"))
            return;

        playerInRange = false;

        if (alertPanel != null)
        {
            alertPanel.gameObject.SetActive(false);
        }
    }

    public void StartNPCDialogue()
    {
        if (oneTimeOnly && hasTriggered)
            return;

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager tidak ditemukan!");
            return;
        }

        if (dialogueData == null)
        {
            Debug.LogError("Dialogue Data kosong!");
            return;
        }

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
        if (moveAfterDialogue)
        {
            NPCMover mover = GetComponent<NPCMover>();

            if (mover != null)
            {
                mover.StartMove();
            }
        }

        if (nextNPC != null)
        {
            StartCoroutine(StartNextNPCDialogue());
        }
    }

    IEnumerator StartNextNPCDialogue()
    {
        yield return new WaitForSeconds(0.7f);

        nextNPC.StartForcedDialogue();
    }
}