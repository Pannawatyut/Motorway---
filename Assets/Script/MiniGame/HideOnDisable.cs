using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnDisable : MonoBehaviour
{
    public void OnDisable()
    {
        // Platform-specific behavior for Android
#if UNITY_ANDROID
        ButtonChangePlayerCanMove.Reset = false;
#else
        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ButtonChangePlayerCanMove.Reset = false;
#endif
    }
}
