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

    // Unlock Object
    private UnlockObject unlockObject;

    // NPC Gone
    private SetNPCGone npcGone;

    void Start()
    {
        // Ambil component UnlockObject
        unlockObject = GetComponent<UnlockObject>();

        // Ambil component SetNPCGone
        npcGone = GetComponent<SetNPCGone>();

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

        // Unlock object
        if (unlockObject != null)
        {
            unlockObject.Unlock();
            Debug.Log("Portal berhasil di-unlock.");
        }

        // Hilangkan NPC
        if (npcGone != null)
        {
            npcGone.RemoveNPC();
            Debug.Log("NPC berhasil dihapus.");
        }

        // Fade Out
        float duration = 0f;

        if (FakeBloomEffect.Instance != null)
        {
            duration = FakeBloomEffect.Instance.fadeOutDuration;
            FakeBloomEffect.Instance.FadeOut();
        }

        // Tunggu fade selesai
        yield return new WaitForSeconds(duration);

        // Load scene async
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