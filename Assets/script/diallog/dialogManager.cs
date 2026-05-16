using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class DialogueData
    {
        public string speaker;

        [TextArea(3, 5)]
        public string text;

        public Sprite pfp;
        public Sprite image;
    }

    [Header("Dialogue")]
    public DialogueData[] dialogues;

    [Header("UI")]
    public Image dialoguePanel;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Image pfpImage;
    public Image bgImage;

    [Header("Input")]
    public KeyCode nextKey = KeyCode.E;

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;

    private int currentIndex = 0;

    private bool isTyping = false;

    private Coroutine typingCoroutine;

    void Start()
    {
        ShowDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(nextKey))
        {
            // Kalau text masih mengetik
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);

                dialogueText.text = dialogues[currentIndex].text;

                isTyping = false;

                return;
            }

            // Kalau sudah selesai mengetik
            NextDialogue();
        }
    }

    void ShowDialogue()
    {
        DialogueData currentDialogue = dialogues[currentIndex];

        // Aktifkan panel
        dialoguePanel.gameObject.SetActive(true);

        // Nama
        nameText.text = currentDialogue.speaker;

        // PFP
        if (currentDialogue.pfp != null)
        {
            pfpImage.sprite = currentDialogue.pfp;
        }

        // Background
        if (currentDialogue.image != null)
        {
            bgImage.sprite = currentDialogue.image;
        }

        // Stop typing lama
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Mulai typing baru
        typingCoroutine = StartCoroutine(TypeText(currentDialogue.text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextDialogue()
    {
        currentIndex++;

        if (currentIndex >= dialogues.Length)
        {
            EndDialogue();
            return;
        }

        ShowDialogue();
    }

    void EndDialogue()
    {
        dialoguePanel.gameObject.SetActive(false);

        Debug.Log("Dialogue selesai");
    }
}