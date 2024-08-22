using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MiniGameGui : MonoBehaviourPunCallbacks
{
    public GameObject UI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            // Check if the local player is the one triggering the warp
            if (other.GetComponent<PhotonView>().IsMine)
            {
                UI.SetActive(true);
            }
        }
    }

    public void CloseMenu()
    {
        UI.SetActive(false);
    }
}
