using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIpopup : MonoBehaviourPun
{
    public Canvas canvasPressF;
    private bool playerInside2;
    private GameObject currentNPC;
    private Canvas map;
    private MouseUIController MouseUIController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UIPopUp"))
        {
            if (photonView.IsMine)
            {
                playerInside2 = true;
                if (photonView.IsMine && playerInside2)
                {
                    canvasPressF.gameObject.SetActive(true);
                }

                currentNPC = other.gameObject;

                // ใช้ GetComponentInChildren<Canvas>() เพื่อหา Canvas ที่เป็นลูกของ GameObject ที่มี Collider และ Tag เป็น "UIPopUp"
                map = currentNPC.GetComponentInChildren<Canvas>(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UIPopUp"))
        {
            if (photonView.IsMine)
            {
                playerInside2 = false;
                canvasPressF.gameObject.SetActive(false);

                // เคลียร์ข้อมูลเมื่อผู้เล่นออกจากพื้นที่
                currentNPC = null;
                map.gameObject.SetActive(false);
                MouseUIController.ResetToEvery();



            }
        }

    }

    void Update()
    {
        if (playerInside2 && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed while player is inside trigger zone!");
            canvasPressF.gameObject.SetActive(false);


            map.gameObject.SetActive(true);

        }
    }

    public void OnButtonPressFLookMap()
    {
        if (playerInside2)
        {
            Debug.Log("F key pressed via UI button while player is inside trigger zone!");
            canvasPressF.gameObject.SetActive(false);

            if (map != null)
            {
                map.gameObject.SetActive(true);
            }
        }
    }
}