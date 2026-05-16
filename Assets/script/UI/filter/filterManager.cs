using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FakeBloomEffect : MonoBehaviour
{
    public Image glowOverlay;

    [Header("Bloom")]
    public float startAlpha = 0.7f;
    public float endAlpha = 0f;

    public float duration = 3f;

    void Start()
    {
        StartCoroutine(BloomFade());
    }

    IEnumerator BloomFade()
    {
        Color color = glowOverlay.color;

        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
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
            endAlpha
        );
    }
}