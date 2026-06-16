using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);

            // Memuat volume yang tersimpan saat game baru dimulai
            float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            audioSource.volume = savedVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        if (soundEffectLibrary == null) return;
        
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if(audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    void Start()
    {
        // Ambil data volume yang tersimpan
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Jika ada slider yang terpasang di scene awal, atur posisinya
        if (sfxSlider != null)
        {
            sfxSlider.value = savedVolume;
            sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        }
    }

    public static void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
        // Menyimpan nilai volume ke memori local
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnValueChanged()
    {
        if (sfxSlider != null)
        {
            SetVolume(sfxSlider.value);
        }
    }

    public void PlayClickSound()
    {
        SoundEffectManager.Play("Click");
    }
}