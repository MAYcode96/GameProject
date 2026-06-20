using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FakeBloomEffect : MonoBehaviour
{
    public static FakeBloomEffect Instance;
    public Image glowOverlay;
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;

    public bool IsFading { get; private set; }

    Coroutine fadeCoroutine;

    void Awake()
    {
        Instance = this;
    }

    // FADE IN
    public void FadeIn()
    {
        StartFade(1f, 0f, fadeInDuration);
    }

    public void FadeIn(float duration)
    {
        StartFade(1f, 0f, duration);
    }

    // FADE OUT

    public void FadeOut()
    {
        StartFade(0f, 1f, fadeOutDuration);
    }

    public void FadeOut(float duration)
    {
        StartFade(0f, 1f, duration);
    }

    // CUSTOM FADE
 
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
        IsFading = true;

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

        if (to <= 0f)
        {
            glowOverlay.gameObject.SetActive(false);
        }

        IsFading = false;
    }
}