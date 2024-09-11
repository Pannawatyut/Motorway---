using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerInteraction : MonoBehaviourPun
{
    public Canvas canvasPressF;
    public bool playerInside;
    private GameObject currentNPC;
    public Canvas Dialog;
    private int number = 0;
    public static bool PressF;
    public BasicBehaviour _MovementScript;
    public MoveBehaviour _MoveBehaviorScript;
    public ThirdPersonOrbitCamBasic cam;
    private bool isCursorVisible = false; // Track the cursor state
    private SoundManager _soundManager; // Reference to the SoundManager

    private void Start()
    {
    #if !UNITY_ANDROID
        //DisableCursor();
    #endif
        number = 0;
        _soundManager = SoundManager.instance; // Ensure SoundManager is correctly referenced
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
                PressF = false;
                ResetCameraSettings();
            }
        }
    }

    void Update()
    {
        if (playerInside)
        {
            if (Input.GetKeyDown(KeyCode.F) && ButtonChangePlayerCanMove.Reset == false && PressF == false)
            {
                //ปิด UI Setting
                SettingUI.turnOff = true;
                
                //โชว์ Dialog
                if (Dialog != null)
                {
                    Dialog.gameObject.SetActive(true);
                }

                //Lock cam
                //SwitchCursor();
                PressF = true;
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
                //EnableCursor();
            }
        }
        else
        {
            SettingUI.turnOff = false;
        }

        if (ButtonChangePlayerCanMove.Reset)
        {
            ResetCameraSettings();
        }
        
        
        //ปุ่ม Enable และ Disable mouse 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorVisible)
            {

                DisableCursor();
            }
            else
            {
                EnableCursor();
                
            }
            isCursorVisible = !isCursorVisible; // Toggle cursor visibility state
        }

        if (isCursorVisible == false)
        {
            DisableCursor();
        }
        else if (isCursorVisible == true)
        {
            EnableCursor();         
        }
    }

    public void SwitchCursor() 
    {
        isCursorVisible = !isCursorVisible;
    }

    private void ResetCameraSettings()
    {
        PressF = false;
    }

    private void EnableCursor()
    {
        //cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cam.horizontalAimingSpeed = 0;
        cam.verticalAimingSpeed = 0;
        _MoveBehaviorScript.RemoveVerticalVelocity();
        //_MovementScript.enabled = false;
        //_MoveBehaviorScript.enabled = false;


    }

    private void DisableCursor()
    {
        //cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.horizontalAimingSpeed = 6;
        cam.verticalAimingSpeed = 6;
        //_MovementScript.enabled = true;
        //_MoveBehaviorScript.enabled = true;

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
                return SoundManager.SoundName.NPC1; 
            case "NPC2":
                return SoundManager.SoundName.NPC2;
            case "NPC3":
                return SoundManager.SoundName.NPC3;
            case "NPC4":
                return SoundManager.SoundName.NPC4;
            case "NPC5":
                return SoundManager.SoundName.NPC5;
            case "NPC6":
                return SoundManager.SoundName.NPC6;
            case "NPC7":
                return SoundManager.SoundName.NPC7;
            case "NPC8":
                return SoundManager.SoundName.NPC8;
            case "NPC9":
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
                
            //โชว์ Dialog
            if (Dialog != null)
            {
                Dialog.transform.GetChild(0).gameObject.SetActive(true);
            }
                
            //Lock cam
            PressF = true;
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
