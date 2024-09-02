using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    public Texture2D cursor_normal;
    public Vector2 normalCursorHotSpot;
    public int cursorSizeX = 16; // Your cursor size x
    public int cursorSizeY = 16; // Your cursor size y
    public Texture2D cursor_OnButton;
    public Vector2 onButtonCursorHotSpot;

    void Start()
    {
        SetCursor(cursor_normal, normalCursorHotSpot);
    }

    public void OnButtonCursorEnter()
    {
        SetCursor(cursor_OnButton, onButtonCursorHotSpot);
    }

    public void OnButtonCursorExit()
    {
        SetCursor(cursor_normal, normalCursorHotSpot);
    }

    public void OnButtonClick()
    {
        SetCursor(cursor_normal, normalCursorHotSpot);
    }

    private void SetCursor(Texture2D cursorTexture, Vector2 hotspot)
    {
        // Resize the cursor texture
        Texture2D resizedCursor = ResizeTexture(cursorTexture, cursorSizeX, cursorSizeY);
        Cursor.SetCursor(resizedCursor, hotspot, CursorMode.ForceSoftware);
    }

    private Texture2D ResizeTexture(Texture2D source, int width, int height)
    {
        // Create a temporary render texture
        RenderTexture rt = RenderTexture.GetTemporary(width, height);

        // Set up the camera to render the source texture to the render texture
        RenderTexture.active = rt;
        GL.Clear(true, true, Color.clear);
        Graphics.Blit(source, rt);

        // Read the render texture into a new texture2D
        Texture2D result = new Texture2D(width, height);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();

        // Clean up
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
}