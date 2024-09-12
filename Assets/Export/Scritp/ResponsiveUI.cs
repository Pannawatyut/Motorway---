using UnityEngine;
using UnityEngine.UI;

public class ResponsiveUI : MonoBehaviour
{
    public CanvasScaler canvasScaler;  // Reference to the CanvasScaler component
    private float baseWidth = 1920f;   // Reference resolution width
    private float baseHeight = 1080f;  // Reference resolution height
    private float baseAspect = 16f / 9f;  // 16:9 aspect ratio

    private void Start()
    {
        AdjustCanvas();
    }

    private void Update()
    {
        AdjustCanvas();  // Continuously update to handle screen resolution changes
    }

    void AdjustCanvas()
    {
        // Get the current screen resolution
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the aspect ratio of the current screen
        float currentAspect = screenWidth / screenHeight;

        // Set the reference resolution to 1920x1080 (fixed aspect ratio)
        canvasScaler.referenceResolution = new Vector2(baseWidth, baseHeight);

        // Adjust the UI scale to ensure it fits entirely on screen while preserving aspect ratio
        if (currentAspect >= baseAspect)
        {
            // Screen is wider or equal to 16:9, match height
            canvasScaler.matchWidthOrHeight = 1;  // Match height to fit the screen
        }
        else
        {
            // Screen is taller than 16:9, match width
            canvasScaler.matchWidthOrHeight = 0;  // Match width to fit the screen
        }

        // Ensure the scaling mode is set to scale with the screen size
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }
}
