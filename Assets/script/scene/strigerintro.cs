using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyChangeScene : MonoBehaviour
{
    public string sceneName;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name belum diisi!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}