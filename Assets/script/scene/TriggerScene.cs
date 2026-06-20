using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTrigger : MonoBehaviour
{
    public string sceneName;
    public bool autoMove = false;
    public KeyCode interactKey = KeyCode.W;

    public Image alertPanel;

    private bool playerInRange;
    private bool isLoading;

    private UnlockObject unlockObject;
    private SetNPCGone npcGone;

    void Start()
    {
        unlockObject = GetComponent<UnlockObject>();
        npcGone = GetComponent<SetNPCGone>();

        if (alertPanel != null)
            alertPanel.gameObject.SetActive(false);

        if (FakeBloomEffect.Instance != null)
            FakeBloomEffect.Instance.FadeIn();
    }

    void Update()
    {
        if (!playerInRange || isLoading) return;

        if (autoMove)
        {
            StartCoroutine(LoadSceneRoutine());
        }
        else if (Input.GetKeyDown(interactKey))
        {
            StartCoroutine(LoadSceneRoutine());
        }
    }

    IEnumerator LoadSceneRoutine()
    {
        isLoading = true;

        if (unlockObject != null)
        {
            unlockObject.Unlock();
            Debug.Log("Portal berhasil di-unlock.");
        }

        if (npcGone != null)
        {
            npcGone.RemoveNPC();
            Debug.Log("NPC berhasil dihapus.");
        }

        float duration = 0f;
        if (FakeBloomEffect.Instance != null)
        {
            duration = FakeBloomEffect.Instance.fadeOutDuration;
            FakeBloomEffect.Instance.FadeOut();
        }

        yield return new WaitForSeconds(duration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            playerInRange = true;
            if (alertPanel != null)
                alertPanel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            playerInRange = false;
            if (alertPanel != null)
                alertPanel.gameObject.SetActive(false);
        }
    }
}