using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);

            // Memuat volume yang tersimpan saat game baru dimulai
            float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            audioSource.volume = savedVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Ambil data volume yang tersimpan
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);

        // Jika ada slider yang terpasang di scene awal, atur posisinya
        if (musicSlider != null)
        {
            musicSlider.value = savedVolume;
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
        }

        if(backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
    }

    public static void SetVolume(float volume)
    {
        if (Instance != null && Instance.audioSource != null)
        {
            Instance.audioSource.volume = volume;
        }
        // Menyimpan nilai volume ke memori local
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if(audioClip != null)
        {
            audioSource.clip = audioClip;
        }
        if(audioSource.clip != null)
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    public void PauseBackgroundMusic()
    {
        audioSource.Pause();
    }

    public static void ChangeBGM(AudioClip laguBaru, bool resetLagu = true)
    {
        if (Instance != null)
        {
            // Tips pro: Jika lagu yang mau diputar sudah sama dan sedang berbunyi, 
            // abaikan saja agar musik tidak terpotong atau mengulang dari awal secara aneh.
            if (Instance.audioSource.clip == laguBaru && Instance.audioSource.isPlaying) return;

            Instance.PlayBackgroundMusic(resetLagu, laguBaru);
        }
    }
}