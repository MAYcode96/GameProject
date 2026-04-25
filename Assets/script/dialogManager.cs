using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

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

    public void StartDialogue(DialogueSequence dialogue)
    {
        currentDialogue = dialogue;
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

        nameText.text = line.characterName;
        dialogueText.text = line.text;

        // ?? Trigger event (cutscene hook)
        line.onLineStart?.Invoke();

        // ?? Fokus ke speaker (optional)
        if (line.speaker != null)
        {
            Debug.Log("Focus ke: " + line.speaker.name);
            // nanti bisa sambung ke camera system
        }
    }

    void EndDialogue()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
    }

    public bool IsPlaying() => isPlaying;
}