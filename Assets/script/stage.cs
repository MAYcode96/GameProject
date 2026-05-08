using UnityEngine;

public class Stage : MonoBehaviour
{
    public static Stage Instance;

    public int storyStage = 0;

    void Awake()
    {
        Instance = this;
    }

    public void SetStage(int stage)
    {
        storyStage = stage;
        Debug.Log("Stage sekarang: " + storyStage);
    }
}