using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CursorManagerScript : MonoBehaviourPun
{
    public static CursorManagerScript Instance;
    public ThirdPersonOrbitCamBasic cam;

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

    private void Update()
    {
        cam = FindAnyObjectByType<ThirdPersonOrbitCamBasic>();
    }

    public void EnableCursor()
    {
#if !UNITY_ANDROID || !UNITY_IOS
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Enable Cursor");

        if (cam != null)
        {
            cam.horizontalAimingSpeed = 0;
            cam.verticalAimingSpeed = 0;
        }
        //_MovementScript.enabled = false;
        //_MoveBehaviorScript.enabled = false;
#endif
    }

    public void DisableCursor()
    {
#if !UNITY_ANDROID || !UNITY_IOS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Disable Cursor");

        if (cam != null)
        {
            cam.horizontalAimingSpeed = 6;
            cam.verticalAimingSpeed = 6;
        }

        ButtonChangePlayerCanMove.Reset = false;
        //_MovementScript.enabled = true;
        //_MoveBehaviorScript.enabled = true;#else
        // Cursor.lockState = CursorLockMode.Locked;
#else
        if (cam != null)
        {
            cam.horizontalAimingSpeed = 6;
            cam.verticalAimingSpeed = 6;
        }
        ButtonChangePlayerCanMove.Reset = false;
#endif
    }
}
