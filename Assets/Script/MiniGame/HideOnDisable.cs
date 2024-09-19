using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnDisable : MonoBehaviour
{
    public void OnDisable()
    {
#if !UNITY_ANDROID || !UNITY_IOS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ButtonChangePlayerCanMove.Reset = false;
#endif
        Cursor.visible = false;
        ButtonChangePlayerCanMove.Reset = false;
    }
}
