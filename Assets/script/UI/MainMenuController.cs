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
        PlayClickSound();
        
        // Hapus file lama agar data benar-benar baru
        System.IO.File.Delete(Application.persistentDataPath + "/save.json");
        
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
        PlayClickSound();
        
        string path = Application.persistentDataPath + "/save.json";
        
        if (System.IO.File.Exists(path))
        {
            // Ada file save, lanjut ke scene terakhir
            // Kita perlu memuat data dulu agar GameManager tahu harus ke scene mana
            GameData loadedData = SaveSystem.Load(); 
            if (loadedData != null)
            {
                SceneManager.LoadScene(loadedData.lastScene);
            }
        }
        else
        {
            // Tidak ada file save
            noSavedGameDialog.SetActive(true);
        }
    }
    
    public void Quit()
    {
        PlayClickSound(); // Memanggil suara klik
        Application.Quit();
    }
}