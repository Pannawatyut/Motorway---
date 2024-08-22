using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Warp : MonoBehaviourPunCallbacks
{
    //public Transform warpTarget; 
    //public Camera playerCamera; 
    public string SceneGame;
    private bool inside = false;
    public GameObject pressF;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                inside = true;
                // Load the game scene
                //PhotonNetwork.LoadLevel(SceneGame);
                pressF.gameObject.SetActive(true);
               
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
            pressF.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(inside && Input.GetKeyDown(KeyCode.F))
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(SceneGame);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneGame);
    }


}