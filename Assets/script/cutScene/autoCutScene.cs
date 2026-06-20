using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoCutsceneImage : MonoBehaviour
{
    public enum TriggerType
    {
        Auto,
        PressKey
    }

    
    public TriggerType triggerType = TriggerType.Auto;

    public LayerMask playerLayer;

    public KeyCode interactKey = KeyCode.W;

    public GameObject pressKeyUI;

    public GameObject persistentBackground;

    public CanvasGroup[] cutscenePages;

    public float fadeDuration = 1f;

    public float showDuration = 3f;

    public float[] pageDurations;

    public bool triggerOnce = false;

    public GameObject objectToActivate;

    public bool loadSceneAfterCutscene = false;
    public string nextSceneName;

    private bool playerInRange;
    private bool hasTriggered;
    private bool isShowing;
    private Coroutine fadeCoroutine;

    void Start()
    {
        foreach (CanvasGroup page in cutscenePages)
        {
            if (page == null)
                continue;

            page.alpha = 0f;
            page.interactable = false;
            page.blocksRaycasts = false;
            page.gameObject.SetActive(false);
        }

        if (pressKeyUI != null)
            pressKeyUI.SetActive(false);

        if (objectToActivate != null)
            objectToActivate.SetActive(false);

        if (persistentBackground != null)
            persistentBackground.SetActive(false);
    }

    void Update()
    {
        if (triggerType != TriggerType.PressKey)
            return;

        if (!playerInRange)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            if (triggerOnce && hasTriggered)
                return;

            if (isShowing)
                return;

            if (pressKeyUI != null)
                pressKeyUI.SetActive(false);

            hasTriggered = true;

            StartCutscene();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) == 0)
            return;

        playerInRange = true;

        if (triggerType == TriggerType.Auto)
        {
            if (triggerOnce && hasTriggered)
                return;

            if (isShowing)
                return;

            hasTriggered = true;

            StartCutscene();
        }
        else
        {
            if (pressKeyUI != null)
                pressKeyUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) == 0)
            return;

        playerInRange = false;

        if (pressKeyUI != null)
            pressKeyUI.SetActive(false);
    }

    void StartCutscene()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(CutsceneRoutine());
    }

    IEnumerator CutsceneRoutine()
    {
        if (cutscenePages == null || cutscenePages.Length == 0)
            yield break;

        isShowing = true;

        if (persistentBackground != null)
            persistentBackground.SetActive(true);

        for (int i = 0; i < cutscenePages.Length; i++)
        {
            CanvasGroup page = cutscenePages[i];

            if (page == null)
                continue;

            page.gameObject.SetActive(true);
            page.alpha = 0f;

            yield return StartCoroutine(
                FadeCanvasGroup(page, 0f, 1f)
            );

            float duration = showDuration;

            if (pageDurations != null &&
                i < pageDurations.Length)
            {
                duration = pageDurations[i];
            }

            yield return new WaitForSeconds(duration);

            yield return StartCoroutine(
                FadeCanvasGroup(page, 1f, 0f)
            );

            page.gameObject.SetActive(false);
        }

        if (persistentBackground != null)
            persistentBackground.SetActive(false);

        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        isShowing = false;

        if (loadSceneAfterCutscene &&
            !string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator FadeCanvasGroup(
        CanvasGroup canvasGroup,
        float startAlpha,
        float endAlpha)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                timer / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}