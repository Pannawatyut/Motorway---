using UnityEngine;
using UnityEngine.UI;

public class MouseWheelScrollbar : MonoBehaviour
{
    public ScrollRect scrollRect; // Reference to the ScrollRect component
    public RectTransform imageRectTransform; // RectTransform of the Image

    private RectTransform contentRectTransform; // RectTransform of the Content

    void Start()
    {
        // Initialize references
        contentRectTransform = scrollRect.content;

        // Optionally, adjust the initial scroll position if needed
        UpdateScrollLimits();
    }

    void Update()
    {
        // Continuously adjust scroll limits as needed
        UpdateScrollLimits();
    }

    void UpdateScrollLimits()
    {
        // Get sizes
        Vector2 imageSize = imageRectTransform.rect.size;
        Vector2 contentSize = contentRectTransform.rect.size;
        Vector2 viewportSize = scrollRect.viewport.rect.size;

        // Calculate new scroll limits based on image size
        float horizontalLimit = Mathf.Max(0, contentSize.x - viewportSize.x);
        float verticalLimit = Mathf.Max(0, contentSize.y - viewportSize.y);

        // Limit the scroll position
        Vector2 scrollPosition = scrollRect.normalizedPosition;
        scrollPosition.x = Mathf.Clamp(scrollPosition.x, 0, horizontalLimit);
        scrollPosition.y = Mathf.Clamp(scrollPosition.y, 0, verticalLimit);

        scrollRect.normalizedPosition = scrollPosition;
    }
}