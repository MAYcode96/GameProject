using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTrigger : MonoBehaviour
{
    [Header("Scene")]
    public string sceneName;

    [Header("UI")]
    public Image alertPanel;

    private bool playerInRange;
    private bool isLoading;

    // Ambil script UnlockObject
    private UnlockObject unlockObject;

    void Start()
    {


        // Ambil component UnlockObject di object ini
        unlockObject = GetComponent<UnlockObject>();

        // Matikan panel alert di awal
        if (alertPanel != null)
        {
            alertPanel.gameObject.SetActive(false);
        }

        // Fade in scene
        if (FakeBloomEffect.Instance != null)
        {
            FakeBloomEffect.Instance.FadeIn();
        }
    }

    void Update()
    {
        if (playerInRange && !isLoading && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(LoadSceneRoutine());
        }
    }

    IEnumerator LoadSceneRoutine()
    {
        isLoading = true;

        // Unlock object & paksa GameManager nulis ke JSON disk
        if (unlockObject != null)
        {
            unlockObject.Unlock();
            Debug.Log("Portal berhasil di-unlock.");
        }

        // Ambil durasi fade
        float duration = 0f;
        if (FakeBloomEffect.Instance != null)
        {
            duration = FakeBloomEffect.Instance.fadeOutDuration;
            FakeBloomEffect.Instance.FadeOut();
        }

        // Tunggu fade selesai (memberikan waktu yang sangat cukup bagi SaveSystem untuk menulis file)
        yield return new WaitForSeconds(duration);

        // Gunakan Async agar perpindahan scene lebih aman dan tidak memutus proses I/O secara kasar
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            playerInRange = true;

            if (alertPanel != null)
            {
                alertPanel.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            playerInRange = false;

            if (alertPanel != null)
            {
                alertPanel.gameObject.SetActive(false);
            }
        }
    }
}