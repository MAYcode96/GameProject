using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class creditsScript : MonoBehaviour
{
    public float scrollSpeed = 40f;
    public string mainMenuSceneName = "MainMenu";

    [Header("Background")]
    public CanvasGroup backgroundCanvasGroup;
    public float fadeDuration = 1.5f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private float exitThreshold;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            exitThreshold = (canvas.GetComponent<RectTransform>().rect.height / 2)
                          + (rectTransform.rect.height * rectTransform.pivot.y);
        }
        else
        {
            exitThreshold = 1000f;
        }

        if (backgroundCanvasGroup != null)
        {
            backgroundCanvasGroup.alpha = 0f;
            StartCoroutine(FadeInBackground());
        }
        else
        {
            Debug.LogWarning("Background Canvas Group belum di-assign!");
        }
    }

    IEnumerator FadeInBackground()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            backgroundCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        backgroundCanvasGroup.alpha = 1f;
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        if (rectTransform.anchoredPosition.y >= exitThreshold)
        {
            EndCredits();
        }
    }

    void EndCredits()
    {
        string savePath = Application.persistentDataPath + "/save.json";
        if (System.IO.File.Exists(savePath))
        {
            System.IO.File.Delete(savePath);
        }
        SceneManager.LoadScene(mainMenuSceneName);
    }
}