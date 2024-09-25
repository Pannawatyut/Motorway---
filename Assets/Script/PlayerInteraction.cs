using UnityEngine;
using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;

public class PlayerInteraction : MonoBehaviourPun
{
    public Canvas canvasPressF;
    public bool playerInside;
    private GameObject currentNPC;
    public Canvas Dialog;
    public MouseUIController _MapInspector;
    private int number = 0;
    public BasicBehaviour _MovementScript;
    public MoveBehaviour _MoveBehaviorScript;
    public ThirdPersonOrbitCamBasic cam;
    private bool isCursorVisible = false; // Track the cursor state
    private SoundManager _soundManager; // Reference to the SoundManager
    public CursorManagerScript _CursorManager;
    private void Start()
    {
    #if !UNITY_ANDROID
        //DisableCursor();
    #endif
        number = 0;
        _soundManager = SoundManager.instance; // Ensure SoundManager is correctly referenced
        _CursorManager = CursorManagerScript.Instance;

        if(photonView.IsMine)
        {
            cam.tag = "MainCamera";
        }
        else
        {
            cam.tag = "Untagged";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if (photonView.IsMine)
            {
                number++;
                playerInside = true;
                if (photonView.IsMine && playerInside)
                {
                    canvasPressF.gameObject.SetActive(true);
                }

                currentNPC = other.gameObject;
                ButtonChangePlayerCanMove.Reset = false;
                Dialog = currentNPC.GetComponentInChildren<Canvas>(true);

                // Fetch or assign the sound name based on the NPC
                string npcName = currentNPC.name; // Or any other unique identifier
                SoundManager.SoundName npcSound = GetSoundNameForNPC(npcName);
                
                // Update the SoundManager's current NPC sound
                if (_soundManager != null)
                {
                    _soundManager.SetCurrentNPCSound(npcSound);
                }
            }
        }

        else if (other.CompareTag("UIPopUp"))
        {
            if (photonView.IsMine)
            {
                number++;
                playerInside = true;
                if (photonView.IsMine && playerInside)
                {
                    canvasPressF.gameObject.SetActive(true);
                }

                currentNPC = other.gameObject;
                ButtonChangePlayerCanMove.Reset = false;
                Dialog = currentNPC.GetComponentInChildren<Canvas>(true);
                _MapInspector = currentNPC.GetComponentInChildren<MouseUIController>(true);
                // Fetch or assign the sound name based on the NPC
                string npcName = currentNPC.name; // Or any other unique identifier
                SoundManager.SoundName npcSound = GetSoundNameForNPC(npcName);

                // Update the SoundManager's current NPC sound
                if (_soundManager != null)
                {
                    _soundManager.SetCurrentNPCSound(npcSound);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if (photonView.IsMine)
            {
                if (Dialog != null)
                {
                    Dialog.gameObject.SetActive(false);
                }

                playerInside = false;
                canvasPressF.gameObject.SetActive(false);
                currentNPC = null;
                Dialog = null; // Reset 'canvas' when exiting trigger
                _MapInspector = null;
                //PressF = false;
                ResetCameraSettings();
            }
        }
        else if (other.CompareTag("UIPopUp"))
        {
            if (photonView.IsMine)
            {
                if (Dialog != null)
                {
                    Dialog.gameObject.SetActive(false);
                }

                playerInside = false;
                canvasPressF.gameObject.SetActive(false);
                currentNPC = null;
                Dialog = null; // Reset 'canvas' when exiting trigger
                _MapInspector = null;
                //PressF = false;
                ResetCameraSettings();
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (_CursorManager == null)
            {
                _CursorManager = FindObjectOfType<CursorManagerScript>();
                if (_CursorManager != null)
                {
                    EnableCursor();
                    _CursorManager.EnableCursor();
                }

            }

            if (playerInside)
            {
                if (Input.GetKeyDown(KeyCode.F) && ButtonChangePlayerCanMove.Reset == false && _MapInspector == null )
                {
                    //ปิด UI Setting
                    SettingUI.turnOff = true;

                    //โชว์ Dialog
                    if (Dialog != null)
                    {
                        Dialog.gameObject.SetActive(true);
                    }

                    //Lock cam
                    SwitchCursor();
                    _CursorManager.EnableCursor();
                    //PressF = true;
                    cam.horizontalAimingSpeed = 0;
                    cam.verticalAimingSpeed = 0;
                    Debug.Log("F key pressed while player is inside trigger zone!");

                    canvasPressF.gameObject.SetActive(false);

                    Dialog.gameObject.SetActive(true);

                    // Enable mouse cursor when F is pressed
                    EnableCursor();

                    ButtonChangePlayerCanMove.Reset = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.F) && ButtonChangePlayerCanMove.Reset == false && _MapInspector != null)
            {
                //ปิด UI Setting
                SettingUI.turnOff = true;

                //โชว์ Dialog
                if (Dialog != null)
                {
                    Dialog.gameObject.SetActive(true);
                }

                //Lock cam
                SwitchCursor();
                _CursorManager.EnableCursor();
                //PressF = true;
                cam.horizontalAimingSpeed = 0;
                cam.verticalAimingSpeed = 0;
                Debug.Log("F key pressed while player is inside trigger zone!");

                canvasPressF.gameObject.SetActive(false);

                Dialog.gameObject.SetActive(true);
                _MapInspector.isFollowingMouse = true;

                // Enable mouse cursor when F is pressed
                EnableCursor();

                ButtonChangePlayerCanMove.Reset = true;
            }
            else
            {
                SettingUI.turnOff = false;
            }
      

            //ปุ่ม Enable และ Disable mouse 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isCursorVisible)
                {
                    _CursorManager.DisableCursor();
                }
                else
                {
                    _CursorManager.EnableCursor();

                }
                isCursorVisible = !isCursorVisible; // Toggle cursor visibility state
            }

        }

    }

    public void SwitchCursor() 
    {
        isCursorVisible = !isCursorVisible;
    }

    private void ResetCameraSettings()
    {
        //PressF = false;
    }

    private void EnableCursor()
    {
        #if !UNITY_ANDROID || !UNITY_IOS
        //cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cam.horizontalAimingSpeed = 0;
        cam.verticalAimingSpeed = 0;
        _MoveBehaviorScript.RemoveVerticalVelocity();
        //_MovementScript.enabled = false;
        //_MoveBehaviorScript.enabled = false;
        #endif

    }

    private void DisableCursor()
    {
        #if !UNITY_ANDROID || !UNITY_IOS
        //cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.horizontalAimingSpeed = 6;
        cam.verticalAimingSpeed = 6;
        //_MovementScript.enabled = true;
        //_MoveBehaviorScript.enabled = true;
        #endif

    }

    IEnumerator WaitForSoundToFinish()
    {
        while (_soundManager.GetCurrentSoundEffectPlaying())
        {
            yield return null;
        }
    }

    private SoundManager.SoundName GetSoundNameForNPC(string npcName)
    {
        switch (npcName)
        {
            case "NPC1":
                NPC_APICounter.Instance._API_Caller_NPC(1);
                return SoundManager.SoundName.NPC1;
            case "NPC2":
                NPC_APICounter.Instance._API_Caller_NPC(2);
                return SoundManager.SoundName.NPC2;
            case "NPC3":
                NPC_APICounter.Instance._API_Caller_NPC(3);
                return SoundManager.SoundName.NPC3;
            case "NPC4":
                NPC_APICounter.Instance._API_Caller_NPC(4);
                return SoundManager.SoundName.NPC4;
            case "NPC5":
                NPC_APICounter.Instance._API_Caller_NPC(5);
                return SoundManager.SoundName.NPC5;
            case "NPC6":
                NPC_APICounter.Instance._API_Caller_NPC(6);
                return SoundManager.SoundName.NPC6;
            case "NPC7":
                NPC_APICounter.Instance._API_Caller_NPC(7);
                return SoundManager.SoundName.NPC7;
            case "NPC8":
                NPC_APICounter.Instance._API_Caller_NPC(8);
                return SoundManager.SoundName.NPC8;
            case "NPC9":
                NPC_APICounter.Instance._API_Caller_NPC(9);
                return SoundManager.SoundName.NPC9;
            case "MapInspect":
                NPC_APICounter.Instance._API_Caller_NPC(10);
                return SoundManager.SoundName.NPC9;

            default:
                return SoundManager.SoundName.NPC1; // Default sound
        }

    }

    public void OnButtonPressFMobile()
    {
        if (playerInside)
        {
            //ปิด UI Setting
            SettingUI.turnOff = true;

            ButtonChangePlayerCanMove.Reset = true;

            //โชว์ Dialog
            if (Dialog != null)
            {
                Dialog.transform.GetChild(0).gameObject.SetActive(true);
            }
                
            //Lock cam
            //PressF = true;
            cam.horizontalAimingSpeed = 0;
            cam.verticalAimingSpeed = 0;
            Debug.Log("F key pressed while player is inside trigger zone!");

            canvasPressF.gameObject.SetActive(false);

            // Play NPC talk sound effect using SoundManager
            if (_soundManager != null)
            {
                _soundManager.PlaySoundEffect(_soundManager.GetCurrentNPCSound());
                StartCoroutine(WaitForSoundToFinish());

                if (Dialog != null)
                {
                    Dialog.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("No valid 'canvas' GameObject found for NPC.");
                }
            }
            else
            {
                Debug.LogWarning("SoundManager instance is not assigned.");
            }

            // Enable mouse cursor when F is pressed
            EnableCursor();
        }
       
    }
}
