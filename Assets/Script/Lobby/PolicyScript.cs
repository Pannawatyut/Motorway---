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
    public GameObject _ChatBox;

    public bool _isForPdpa;

    void Start()
    {
        // Add listener to the toggle event
        agreeToggle.onValueChanged.AddListener(delegate {
            AgreeCheck(agreeToggle);
        });

        if (LoginManager.Instance._isStarter)
        {
            this.gameObject.SetActive(false);
            _ChatBox.SetActive(true);

            CursorManagerScript.Instance.DisableCursor();
        }
        else
        {
            this.gameObject.SetActive(true);
            _ChatBox.SetActive(false);

            CursorManagerScript.Instance.EnableCursor();
        }
        LoginManager.Instance._isStarter = true;

        if (_loginManager == null)
        {
            _loginManager = FindAnyObjectByType<LoginManager>();
        }

        if (_loginManager != null)
        {
            Parent.SetActive(true);
        }

        CheckforProfile();

    }


    public GameObject _PolicyBTM;
    public void AgreeCheck(Toggle toggle)
    {
        isAgree = toggle.isOn;
        _isForPdpa = toggle.isOn;
        _PolicyBTM.GetComponent<Button>().interactable = toggle.isOn;
        Debug.Log("Agreement status: " + isAgree);
    }

    public void CheckforProfile()
    {
        Debug.Log("CHECK PROFILE");

        if (_loginManager != null) 
        {
            if (_loginManager._Account.first_name == "-" || _loginManager._Account.last_name == "-")
            {
                PDPA_Profile.SetActive(true);
                Debug.Log("CHECK PROFILE - A");
            }
            else
            {
                CheckforQuestionaire();
                PDPA_Profile.SetActive(false);
                Debug.Log("CHECK PROFILE - B");

            }
        }

    }

    public void CheckforQuestionaire()
    {
        if (_loginManager._Account.is_questionnaire == 0)
        {       
            QuestionairePage.SetActive(true);
            Debug.Log("CHECK -> Q/A -> OK");
        }
        else
        {
            Parent.SetActive(false);
            _ChatBox.SetActive(true);
            CursorManagerScript.Instance.DisableCursor();

            Debug.Log("CHECK -> Q/A -> fAILED");
        }
    }

    public void Submit()
    {
        if (isAgree)
        {
            PolicyPage.SetActive(false);
            ProfilePage.SetActive(true);
            _CloseInfoBtm.SetActive(false);
            _CloseEditBtm.SetActive(false);
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

    public GameObject _CloseInfoBtm;
    public GameObject _CloseEditBtm;
}
