using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public DialogueSequence dialogueData;

    [Header("Optional")]
    public string targetScene;

    [Header("Trigger Once")]
    public bool triggerOnce = true;

    private bool hasTriggered;

    void Start()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (triggerOnce && hasTriggered)
            return;

        hasTriggered = true;

        DialogueManager.Instance.StartDialogue(dialogueData, targetScene);
    }
}