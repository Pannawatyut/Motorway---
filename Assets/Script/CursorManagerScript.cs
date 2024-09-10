using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManagerScript : MonoBehaviour
{

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
}
