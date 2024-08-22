using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsvieUI : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public Vector2 referenceResolution = new Vector2(1920, 1080); // ???????????????????? Canvas ????????????

    void Start()
    {
        AdjustCanvasScaler();
    }

    void AdjustCanvasScaler()
    {
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }
}
