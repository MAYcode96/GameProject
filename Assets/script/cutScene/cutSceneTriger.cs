using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueSequence dialogueData;

    [Header("Optional")]
    public string targetScene;

    [Header("Trigger Once")]
    public bool triggerOnce = true;

    [Header("Fade Stage")]
    public FadeStage fadeStage;

    private bool hasTriggered;

    IEnumerator Start()
    {
        yield return null;

        if (fadeStage != null)
        {
            while (!fadeStage.IsFinished())
            {
                yield return null;
            }
        }

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