using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;

    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    public Image pfpImage;
    public Image backgroundImage;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    [Header("Input")]
    public KeyCode nextKey = KeyCode.Space;

    private DialogueSequence currentDialogue;

    private int currentIndex;

    private bool isTyping;

    public bool isDialogueOpen;

    private bool canPressNext;

    private Coroutine typingCoroutine;

    private string targetScene;

    private NPC currentNPC;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);

        isDialogueOpen = false;

        if (FakeBloomEffect.Instance != null)
        {
            FakeBloomEffect.Instance.FadeIn();
        }
    }

    void Update()
    {
        if (!isDialogueOpen)
            return;

        if (currentDialogue == null)
            return;

        if (!canPressNext)
            return;

        if (Input.GetKeyDown(nextKey))
        {
            if (isTyping)
            {
                SkipTyping();
            }
            else
            {
                NextDialogue();
            }
        }
    }

    public void StartDialogue(
        DialogueSequence dialogueData,
        string nextScene = "",
        NPC npc = null
    )
    {
        if (isDialogueOpen)
            return;

        currentDialogue = dialogueData;

        currentIndex = 0;

        targetScene = nextScene;

        currentNPC = npc;

        isDialogueOpen = true;

        canPressNext = false;

        speakerText.text = "";
        dialogueText.text = "";

        if (pfpImage != null)
        {
            pfpImage.sprite = null;
            pfpImage.gameObject.SetActive(false);
        }

        dialoguePanel.SetActive(true);

        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            dialoguePanel.GetComponent<RectTransform>()
        );

        StartCoroutine(StartDialogueRoutine());

        if (dialoguePanel == null)
        {
            Debug.LogWarning("DialoguePanel sudah tidak ada (scene mungkin berubah)");
            return;
        }
    }

    IEnumerator StartDialogueRoutine()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();

        ShowDialogue();

        yield return new WaitForSeconds(0.15f);

        canPressNext = true;
    }

    void ShowDialogue()
    {
        if (currentDialogue == null)
            return;

        if (currentIndex >= currentDialogue.lines.Length)
            return;

        DialogueSequence.DialogueLine line =
            currentDialogue.lines[currentIndex];

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        speakerText.text = line.speaker;

        dialogueText.text = "";

        if (line.pfp != null)
        {
            pfpImage.sprite = line.pfp;
            pfpImage.gameObject.SetActive(true);
        }
        else
        {
            pfpImage.gameObject.SetActive(false);
        }

        if (line.background != null)
        {
            backgroundImage.sprite = line.background;
        }

        Canvas.ForceUpdateCanvases();

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText.text =
            currentDialogue.lines[currentIndex].text;

        isTyping = false;
    }

    void NextDialogue()
    {
        currentIndex++;

        if (currentIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowDialogue();
    }

    void EndDialogue()
    {
        StartCoroutine(EndDialogueRoutine());
    }

    IEnumerator EndDialogueRoutine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        isTyping = false;

        dialoguePanel.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        isDialogueOpen = false;

        if (currentNPC != null)
        {
            currentNPC.OnDialogueFinished();
        }

        currentDialogue = null;

        if (!string.IsNullOrEmpty(targetScene))
        {
            if (FakeBloomEffect.Instance != null)
            {
                FakeBloomEffect.Instance.FadeOut(2f);

                yield return new WaitForSeconds(2f);
            }

            SceneManager.LoadScene(targetScene);
        }
    }
}