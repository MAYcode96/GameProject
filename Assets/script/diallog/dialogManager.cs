using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public UnityEvent onDialogueEnd;

    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public NPC currentNPC;
    private bool firstDialogueDone = false;

    private DialogueSequence currentDialogue;
    private int index;
    private bool isPlaying;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueSequence dialogue, NPC npc = null)
    {
        if (dialogue == null) return;

        currentDialogue = dialogue;
        currentNPC = npc;

        index = 0;
        isPlaying = true;

        dialoguePanel.SetActive(true);
        ShowLine();
    }

    public void NextLine()
    {
        if (!isPlaying) return;

        index++;

        if (index >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        var line = currentDialogue.lines[index];

        nameText.text = line.speakerName;
        dialogueText.text = line.text;

        // 🎬 Event per line
        line.onLineStart?.Invoke();

        // 🎥 Optional camera focus
        if (line.speaker != null)
        {
            Debug.Log("Focus ke: " + line.speaker.name);
        }
    }

    void EndDialogue()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);

        // 🔥 hanya untuk dialog pertama
        if (!firstDialogueDone && currentNPC != null)
        {
            firstDialogueDone = true;

            NPCMover mover = currentNPC.GetComponent<NPCMover>();
            if (mover != null)
            {
                mover.MoveToTarget(); // NPC jalan
            }
        }

        onDialogueEnd?.Invoke();
    }

    public bool IsPlaying() => isPlaying;
}