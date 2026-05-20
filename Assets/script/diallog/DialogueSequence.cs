using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueSequence : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speaker;

        [TextArea(3, 5)]
        public string text;

        public Sprite pfp;

        public Sprite background;
    }

    public DialogueLine[] lines;
}