using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FakeBloomEffect : MonoBehaviour
{
    public static FakeBloomEffect Instance;

    [Header("UI")]
    public Image glowOverlay;

    [Header("Default Duration")]
    public float defaultDuration = 2f;

    Coroutine fadeCoroutine;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // FADE IN
    // terang -> hilang
    // =========================
    public void FadeIn()
    {
        FadeIn(defaultDuration);
    }

    public void FadeIn(float duration)
    {
        StartFade(1f, 0f, duration);
    }

    // =========================
    // FADE OUT
    // hilang -> terang
    // =========================
    public void FadeOut()
    {
        FadeOut(defaultDuration);
    }

    public void FadeOut(float duration)
    {
        StartFade(0f, 1f, duration);
    }

    // =========================
    // CUSTOM FADE
    // =========================
    public void StartFade(float from, float to, float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(
            FadeRoutine(from, to, duration)
        );
    }

    IEnumerator FadeRoutine(float from, float to, float duration)
    {
        float time = 0f;

        Color color = glowOverlay.color;

        glowOverlay.gameObject.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(
                from,
                to,
                time / duration
            );

            glowOverlay.color = new Color(
                color.r,
                color.g,
                color.b,
                alpha
            );

            yield return null;
        }

        glowOverlay.color = new Color(
            color.r,
            color.g,
            color.b,
            to
        );

        // kalau sudah hilang total
        if (to <= 0f)
        {
            glowOverlay.gameObject.SetActive(false);
        }
    }
}