using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class WarpTrigger : MonoBehaviour
{
    public Transform warpLocation;
    public GameObject pressF;
    private bool inside = false;
    private Transform playerTransform; // Store the player's transform here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            playerTransform = other.transform; // Store the player's transform
            pressF.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
            playerTransform = null; // Clear the player's transform
            pressF.SetActive(false);
        }
    }

    private void Update()
    {
        if (inside && playerTransform != null && Input.GetKeyDown(KeyCode.F))
        {
            playerTransform.position = warpLocation.position;
        }
    }
}