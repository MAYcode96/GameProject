using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    public DialogueSequence dialogueData;

    [Header("Optional")]
    public string targetScene;

    [Header("Trigger Once")]
    public bool triggerOnce = true;

    private bool hasTriggered;

    IEnumerator Start()
    {
        // tunggu DialogueManager selesai Awake
        yield return null;
        
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (triggerOnce && hasTriggered)
            return;

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager tidak ditemukan!");
            return;
        }

        hasTriggered = true;

        DialogueManager.Instance.StartDialogue(
            dialogueData,
            targetScene
        );

       
    }
}