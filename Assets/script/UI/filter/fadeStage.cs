using UnityEngine;

public class FadeStage : MonoBehaviour
{
    [Header("UI Reference")]
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    public float fadeSpeed = 2f;

    private bool isFading = false;

    void Start()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("Canvas Group belum dimasukkan ke dalam Inspector di " + gameObject.name);
            return;
        }

        // Paksa UI aktif di awal
        canvasGroup.gameObject.SetActive(true);

        // Paksa terlihat penuh
        canvasGroup.alpha = 1f;

        // Aktifkan interaksi
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void Update()
    {
        if (canvasGroup == null) return;

        if (Input.anyKeyDown && !isFading)
        {
            isFading = true;

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (isFading)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;

            if (canvasGroup.alpha <= 0f)
            {
                canvasGroup.alpha = 0f;
                isFading = false;

                // Kalau mau langsung dimatikan setelah fade selesai
                canvasGroup.gameObject.SetActive(false);
            }
        }
    }
}