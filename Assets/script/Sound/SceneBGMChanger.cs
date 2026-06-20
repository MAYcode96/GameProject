using UnityEngine;

public class SceneBGMChanger : MonoBehaviour
{
    [Header("Pengaturan Musik Scene")]
    [SerializeField] private AudioClip musikKhususScene;

    void Start()
    {
        // Begitu scene dimulai, perintahkan MusicManager untuk ganti lagu
        if (musikKhususScene != null)
        {
            MusicManager.ChangeBGM(musikKhususScene, true);
        }
    }
}