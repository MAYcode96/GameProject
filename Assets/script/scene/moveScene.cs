using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Header("Scene Tujuan")]
    [SerializeField] private string targetScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player"))
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}