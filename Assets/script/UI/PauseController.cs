using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject container;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundEffectManager.Play("Click");
            container.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeButton()
    {
        SoundEffectManager.Play("Click");
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        SoundEffectManager.Play("Click");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}