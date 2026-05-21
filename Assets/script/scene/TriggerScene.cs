using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTrigger : MonoBehaviour
{
    public string sceneName;
    public Image alertPanel;

    private bool playerInRange;
    private bool isLoading;

    void Start()
    {
        alertPanel.gameObject.SetActive(false);

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

        float duration = FakeBloomEffect.Instance.fadeOutDuration;

        FakeBloomEffect.Instance.FadeOut();

        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(sceneName);
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