using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    public string characterName;

    [TextArea(2, 5)]
    public string text;

    public GameObject speaker; // referensi karakter di scene

    public UnityEvent onLineStart; // event saat baris ini mulai
}