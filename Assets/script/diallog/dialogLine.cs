using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea] public string text;

    public UnityEvent onLineStart; // event per line (cutscene, animasi, dll)
    public Transform speaker;     // optional (buat camera focus)
}