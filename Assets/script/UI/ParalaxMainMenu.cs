using UnityEngine;

public class ParalaxMainMenu : MonoBehaviour
{
    public float offsetMultiplier = 50f;
    public float smoothTime = 0.3f;

    private Vector2 startPos;
    private Vector2 velocity;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        Vector2 normalizedPos = new Vector2(
            (mousePos.x / Screen.width) - 0.5f,
            (mousePos.y / Screen.height) - 0.5f
        );

        Vector2 targetPos = startPos + (normalizedPos * offsetMultiplier);

        rectTransform.anchoredPosition = Vector2.SmoothDamp(
            rectTransform.anchoredPosition,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}