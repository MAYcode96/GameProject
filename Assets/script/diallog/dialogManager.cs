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
    public KeyCode nextKey = KeyCode.E;

    private DialogueSequence currentDialogue;
    private int currentIndex;

    private bool isTyping;
    public bool isDialogueOpen;

    private bool canPressNext;

    private Coroutine typingCoroutine;

    private string targetScene;

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

    public void StartDialogue(DialogueSequence dialogueData, string nextScene = "")
    {
        // cegah spam
        if (isDialogueOpen)
            return;

        // data dialog
        currentDialogue = dialogueData;

        currentIndex = 0;

        targetScene = nextScene;

        isDialogueOpen = true;

        canPressNext = false;

        // reset ui
        speakerText.text = "";
        dialogueText.text = "";

        // reset gambar
        if (pfpImage != null)
        {
            pfpImage.sprite = null;
            pfpImage.gameObject.SetActive(false);
        }

        // aktifkan panel
        dialoguePanel.SetActive(true);

        // paksa unity render panel dulu
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            dialoguePanel.GetComponent<RectTransform>()
        );

        // mulai routine
        StartCoroutine(StartDialogueRoutine());
    }

    IEnumerator StartDialogueRoutine()
    {
        // tunggu 1 frame penuh
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();

        // tampilkan dialog pertama
        ShowDialogue();

        // delay input
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

        // stop typing lama
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // speaker
        speakerText.text = line.speaker;

        // kosongkan text dulu
        dialogueText.text = "";

        // profile picture
        if (line.pfp != null)
        {
            pfpImage.sprite = line.pfp;
            pfpImage.gameObject.SetActive(true);
        }
        else
        {
            pfpImage.gameObject.SetActive(false);
        }

        // background
        if (line.background != null)
        {
            backgroundImage.sprite = line.background;
        }

        Canvas.ForceUpdateCanvases();

        // mulai typing
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
        // stop coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // reset state
        isTyping = false;

        isDialogueOpen = false;

        currentDialogue = null;

        // tutup panel
        dialoguePanel.SetActive(false);

        // pindah scene kalau ada
        if (!string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}