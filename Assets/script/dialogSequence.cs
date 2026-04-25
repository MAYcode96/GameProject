using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Sequence")]
public class DialogueSequence : ScriptableObject
{
    public DialogueLine[] lines;
}