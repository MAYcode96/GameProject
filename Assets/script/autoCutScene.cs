using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    [Header("Cutscene Image")]
    public Image cutsceneImage;

    public Sprite cutsceneSprite;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    public float showDuration = 3f;

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
        if (cutsceneImage != null)
        {
            Color color = cutsceneImage.color;
            color.a = 0f;
            cutsceneImage.color = color;

            cutsceneImage.gameObject.SetActive(false);
        }

        // sembunyikan alert awal
        if (pressKeyUI != null)
        {
            pressKeyUI.SetActive(false);
        }


        // object awalnya mati
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

            // hilangkan alert
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
        // cek layer player
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
            // tampilkan alert tombol
            if (pressKeyUI != null)
            {
                pressKeyUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // cek layer player
        if (((1 << other.gameObject.layer) & playerLayer) == 0)
            return;

        playerInRange = false;

        // sembunyikan alert saat keluar area
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
        isShowing = true;

        cutsceneImage.gameObject.SetActive(true);

        if (cutsceneSprite != null)
        {
            cutsceneImage.sprite = cutsceneSprite;
        }

        // FADE IN
        yield return StartCoroutine(FadeImage(0f, 1f));

        // TUNGGU
        yield return new WaitForSeconds(showDuration);

        // FADE OUT
        yield return StartCoroutine(FadeImage(1f, 0f));

        cutsceneImage.gameObject.SetActive(false);

        // aktifkan object setelah cutscene
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        isShowing = false;
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float timer = 0f;

        Color color = cutsceneImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                timer / fadeDuration
            );

            color.a = alpha;

            cutsceneImage.color = color;

            yield return null;
        }

        color.a = endAlpha;

        cutsceneImage.color = color;
    }
}