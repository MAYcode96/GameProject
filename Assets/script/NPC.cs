using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueState[] states;
    public int currentState = 0;

    public DialogueSequence GetCurrentDialogue()
    {
        if (states.Length == 0) return null;

        return states[currentState].sequence;
    }

    public void SetState(int index)
    {
        if (index >= 0 && index < states.Length)
        {
            currentState = index;
        }
    }
}