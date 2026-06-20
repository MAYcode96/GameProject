using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderLinker : MonoBehaviour
{
    public enum SliderType { SFX, BGM }
    public SliderType tipeSlider;

    private Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        if (tipeSlider == SliderType.SFX)
        {
            // Set posisi slider sesuai volume SFX terakhir yang disimpan
            slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            
            // Hubungkan fungsi geser slider ke SoundEffectManager secara otomatis
            slider.onValueChanged.AddListener(delegate { SoundEffectManager.SetVolume(slider.value); });
        }
        else if (tipeSlider == SliderType.BGM)
        {
            // Set posisi slider sesuai volume BGM terakhir yang disimpan
            slider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
            
            // Hubungkan fungsi geser slider ke MusicManager secara otomatis
            slider.onValueChanged.AddListener(delegate { MusicManager.SetVolume(slider.value); });
        }
    }
}