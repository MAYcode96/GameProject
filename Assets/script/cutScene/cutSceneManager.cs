using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public GameObject cutscenePanel;
    public RawImage rawImage;
    public AudioSource audioSource;

    public VideoPlayer videoPlayer;
    public VideoClip cutsceneClip;

    public GameObject creditsPanel;


    void Start()
    {
        cutscenePanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);


        videoPlayer.clip = cutsceneClip;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;

        RenderTexture rt = new RenderTexture(1920, 1080, 0);
        videoPlayer.targetTexture = rt;
        rawImage.texture = rt;

        videoPlayer.loopPointReached += OnCutsceneFinished;
    }

    public void PlayCutscene()
    {
        cutscenePanel.SetActive(true);

        videoPlayer.Play();

        Debug.Log("Cutscene dimulai!");
    }

    void OnCutsceneFinished(VideoPlayer vp)
    {
        Debug.Log("Cutscene selesai!");


        cutscenePanel.SetActive(false);

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }

    }

    void Update()
    {
        if (cutscenePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.Escape))
            {
                videoPlayer.Stop();
                OnCutsceneFinished(videoPlayer);
            }
        }
    }
}