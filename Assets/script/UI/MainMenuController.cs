using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Levels To Load")]
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    
    
    public void PlayClickSound()
    {
        SoundEffectManager.Play("Click");
    }
    

    public void NewGameDialogYes()
    {
        PlayClickSound(); // Memanggil suara klik
        SceneManager.LoadScene("cutScene1");
    }

    public void PlayInformation()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainMenuInfo");
    }

    public void QuitInformation()
    {
        PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameDialogYes()
    {
        PlayClickSound(); // Memanggil suara klik
        if(PlayerPrefs.HasKey("SavedLevel1"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel1");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }
    
    public void Quit()
    {
        PlayClickSound(); // Memanggil suara klik
        Application.Quit();
    }
}