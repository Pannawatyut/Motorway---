using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveUI : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public Vector2 referenceResolution = new Vector2(1920, 1080); 
    public float matchWidthOrHeight = 0.5f; 

    void Start()
    {
        AdjustCanvasScaler();
    }

    void AdjustCanvasScaler()
    {
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
    }
}
