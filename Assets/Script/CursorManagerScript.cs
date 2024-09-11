using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManagerScript : MonoBehaviour
{

    public void DisableCursor()
    {
        //cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnableCursor()
    {
        //cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
}
