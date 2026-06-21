using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        // (agar Main Menu tidak freeze)
        Time.timeScale = 1; 

        // 2. AUTO-SAVE & PINDAH SCENE MENGGUNAKAN GAMEMANAGER
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene("MainMenu");
        }
        else
        {
            Debug.LogWarning("[PauseController] GameManager tidak ditemukan! Pindah scene tanpa save.");
            SceneManager.LoadScene("MainMenu");
        }
    }
}