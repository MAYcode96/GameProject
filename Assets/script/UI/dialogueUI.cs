using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Image backgroundImage;
    public Image portraitImage;

    public void UpdateUI(DialogueLine line)
    {
        nameText.text = line.speaker;
        dialogueText.text = line.text;

        backgroundImage.sprite = line.background;
        portraitImage.sprite = line.portrait;
    }
}