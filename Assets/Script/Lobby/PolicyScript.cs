using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Photon.Pun;
public class PolicyScript : MonoBehaviour
{
    public bool isAgree = false;
    public GameObject Parent;
    public LoginManager _loginManager;
    public GameObject PDPA_Profile;
    public GameObject PolicyPage;
    public GameObject QuestionairePage;
    public GameObject ProfilePage;
    public Toggle agreeToggle;
    public bool FirstTry = false;

    void Start()
    {
        // Add listener to the toggle event
        agreeToggle.onValueChanged.AddListener(delegate {
            AgreeCheck(agreeToggle);
        });
    }
    private void Update()
    {
        if (_loginManager == null)
        {
            _loginManager = FindAnyObjectByType<LoginManager>();
        }

        if (_loginManager != null)
        {
            Parent.SetActive(true);
        }

    }
    public void AgreeCheck(Toggle toggle)
    {
        isAgree = toggle.isOn;
        Debug.Log("Agreement status: " + isAgree);
    }

    public void CheckforProfile()
    {
        if (_loginManager != null) 
        {
            if (_loginManager._Account.first_name == null || _loginManager._Account.last_name == null)
            {
                PDPA_Profile.SetActive(true);
            }
            else
            {
                CheckforQuestionaire();
                PDPA_Profile.SetActive(false);
            }
        }

    }

    public void CheckforQuestionaire()
    {
        if (_loginManager._Account.is_questionnaire == 0)
        {       
            QuestionairePage.SetActive(true);
        }
        else
        {
            Parent.SetActive(false);
        }
    }

    public void Submit()
    {
        if (isAgree)
        {
            PolicyPage.SetActive(false);
            ProfilePage.SetActive(true);
        }
    }

    public void Cancel()
    {
        StartCoroutine(Logout());
    }
    IEnumerator Logout()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(LoginManager.Instance._APIURL + "/api/user/logout"))
        {
            // Add the necessary headers
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", _loginManager._Account.access_token);

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check if the request succeeded or failed
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to Logout: " + request.error);
            }
            else
            {
                Debug.Log("Logout Succesfully: " + request.downloadHandler.text);
                // Disconnect the client from the network
                PhotonNetwork.Disconnect();

                // Wait until the disconnection is complete before loading the main menu scene
                while (PhotonNetwork.IsConnected)
                {
                    yield return null;
                }

                // Load the main menu or login scene
                SceneManager.LoadScene(0);


            }

        }
    }
}
