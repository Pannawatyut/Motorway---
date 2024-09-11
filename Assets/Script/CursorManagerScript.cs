using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManagerScript : MonoBehaviour
{
    public static CursorManagerScript Instance { get; private set; }

    private void Awake()
    {
        // Ensure that there is only one instance of ScoreManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


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
