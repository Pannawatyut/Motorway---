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
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom(); // Leave the current room

            // Wait until Photon confirms that the player has left the room
            while (PhotonNetwork.InRoom || PhotonNetwork.IsConnectedAndReady == false)
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
        // Optionally, you can handle any additional logic here when the room is left.
        // This will be called automatically when PhotonNetwork.LeaveRoom() completes.
        Debug.Log("Left the room, now loading scene...");
    }
}