using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public enum InteractionType
    {
        Auto,
        PressKey
    }

    public Image alertPanel;

    [Header("Dialogue")]
    public DialogueSequence dialogueData;

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
        alertPanel.gameObject.SetActive(false);
    }
    void Update()
    {
        // mode tombol
        if (interactionType != InteractionType.PressKey)
            return;

        // dialog sedang terbuka
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.isDialogueOpen)
            return;

        // tekan tombol
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

        // AUTO
        if (interactionType == InteractionType.Auto)
        {
            if (alertPanel != null)
            {
                alertPanel.gameObject.SetActive(false);
            }

            StartNPCDialogue();
            return;
        }

        // PRESS KEY
        if (alertPanel != null)
        {
            alertPanel.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            playerInRange = false;

            alertPanel.gameObject.SetActive(false);
        }
    }

    void StartNPCDialogue()
    {
        // one time only
        if (oneTimeOnly && hasTriggered)
            return;

        // manager tidak ada
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager tidak ditemukan!");
            return;
        }

        // dialog kosong
        if (dialogueData == null)
        {
            Debug.LogError("Dialogue Data kosong!");
            return;
        }

        hasTriggered = true;

        DialogueManager.Instance.StartDialogue(
            dialogueData,
            targetScene
        );
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
    }
}