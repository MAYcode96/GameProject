using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    public DialogueSequence dialogueData;
    public string targetScene;
    public bool triggerOnce = true;
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

        if (FakeBloomEffect.Instance != null)
        {
            while (FakeBloomEffect.Instance.IsFading)
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