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
    private bool isSceneLoading = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            inside = true;
            pressF.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            inside = false;
            pressF.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (inside && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(LeaveRoomAndLoadScene());
        }
    }

    public void OnButtonPressFMobileWarpScene()
    {
        if (inside)
        {
            StartCoroutine(LeaveRoomAndLoadScene());
        }
    }

    private IEnumerator LeaveRoomAndLoadScene()
    {
        if (isSceneLoading) yield break;  // Prevent further execution if a scene load is already in progress
        isSceneLoading = true;
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("The Scene going to is " + SceneGame);
            PhotonNetwork.Disconnect();

            // Wait until Photon confirms that the player has left the room
            while (!PhotonNetwork.IsConnected)
            {
                yield return null;
            }
        }

        // Small delay to ensure all Photon operations have completed
        yield return new WaitForSeconds(0.2f);
        
        SceneManager.LoadScene(SceneGame); // Now load the scene
    }

    public override void OnLeftRoom()
    {

        //Debug.Log($"Left the room, now loading scene...{SceneGame}");
        isSceneLoading = false;  // Reset flag after leaving the room
    }
}