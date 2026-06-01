using UnityEngine;
using System.Collections;

public class AutoCutsceneImage : MonoBehaviour
{
    public enum TriggerType
    {
        Auto,
        PressKey
    }

    [Header("Trigger Type")]
    public TriggerType triggerType = TriggerType.Auto;

    [Header("Player Layer")]
    public LayerMask playerLayer;

    [Header("Input")]
    public KeyCode interactKey = KeyCode.W;

    [Header("Alert UI")]
    public GameObject pressKeyUI;

    [Header("Cutscene Pages")]
    public CanvasGroup[] cutscenePages;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Show Duration")]
    public float showDuration = 3f;

    [Header("Optional Per Page Duration")]
    public float[] pageDurations;

    [Header("Settings")]
    public bool triggerOnce = false;

    [Header("Activate Object After Cutscene")]
    public GameObject objectToActivate;

    private bool playerInRange;
    private bool hasTriggered;
    private bool isShowing;
    private Coroutine fadeCoroutine;

    void Start()
    {
        // Sembunyikan semua halaman cutscene
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
        {
            pressKeyUI.SetActive(false);
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(false);
        }
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
            {
                pressKeyUI.SetActive(false);
            }

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
            {
                pressKeyUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) == 0)
            return;

        playerInRange = false;

        if (pressKeyUI != null)
        {
            pressKeyUI.SetActive(false);
        }
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

        for (int i = 0; i < cutscenePages.Length; i++)
        {
            CanvasGroup page = cutscenePages[i];

            if (page == null)
                continue;

            page.gameObject.SetActive(true);
            page.alpha = 0f;

            yield return StartCoroutine(FadeCanvasGroup(page, 0f, 1f));

            float duration = showDuration;

            if (pageDurations != null &&
                i < pageDurations.Length)
            {
                duration = pageDurations[i];
            }

            yield return new WaitForSeconds(duration);

            yield return StartCoroutine(FadeCanvasGroup(page, 1f, 0f));

            page.gameObject.SetActive(false);
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        isShowing = false;
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