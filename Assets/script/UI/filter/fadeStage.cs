using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeStage : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup[] uiReferences;

    [Header("Mode")]
    public bool autoMode = false;

    [Header("Auto Settings")]
    public float autoNextTime = 3f;

    [Header("Fade Settings")]
    public float fadeSpeed = 2f;

    [Header("Scene Transition")]
    public bool loadSceneWhenFinished = false;
    public string targetScene;

    private int currentIndex = 0;
    private bool isFading = false;
    private float timer = 0f;

    void Start()
    {
        if (uiReferences == null || uiReferences.Length == 0)
        {
            Debug.LogWarning("FadeStage: Tidak ada UI Reference.");
            return;
        }

        for (int i = 0; i < uiReferences.Length; i++)
        {
            if (uiReferences[i] == null)
                continue;

            uiReferences[i].gameObject.SetActive(i == 0);
            uiReferences[i].alpha = (i == 0) ? 1f : 0f;
            uiReferences[i].interactable = (i == 0);
            uiReferences[i].blocksRaycasts = (i == 0);
        }
    }

    void Update()
    {
        if (uiReferences == null || uiReferences.Length == 0)
            return;

        if (currentIndex >= uiReferences.Length)
            return;

        CanvasGroup currentUI = uiReferences[currentIndex];

        if (currentUI == null)
            return;

        if (!isFading)
        {
            if (autoMode)
            {
                timer += Time.deltaTime;

                if (timer >= autoNextTime)
                {
                    timer = 0f;
                    StartFade(currentUI);
                }
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    StartFade(currentUI);
                }
            }
        }

        if (isFading)
        {
            currentUI.alpha -= fadeSpeed * Time.deltaTime;

            if (currentUI.alpha <= 0f)
            {
                currentUI.alpha = 0f;
                currentUI.gameObject.SetActive(false);

                currentIndex++;

                if (currentIndex < uiReferences.Length)
                {
                    CanvasGroup nextUI = uiReferences[currentIndex];

                    if (nextUI != null)
                    {
                        nextUI.gameObject.SetActive(true);
                        nextUI.alpha = 1f;
                        nextUI.interactable = true;
                        nextUI.blocksRaycasts = true;
                    }
                }
                else
                {
                    if (loadSceneWhenFinished &&
                        !string.IsNullOrEmpty(targetScene))
                    {
                        SceneManager.LoadScene(targetScene);
                    }
                }

                isFading = false;
            }
        }
    }

    void StartFade(CanvasGroup currentUI)
    {
        isFading = true;

        currentUI.interactable = false;
        currentUI.blocksRaycasts = false;
    }

    public bool IsFinished()
    {
        return currentIndex >= uiReferences.Length;
    }

    public bool IsFadingActive()
    {
        return !IsFinished();
    }
}