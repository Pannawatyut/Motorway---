using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.Analytics;

public class ProfileScript : MonoBehaviour
{
    public LoginManager _loginManager;

    [Header("Profile description")]
    public TextMeshProUGUI _firstName;
    public TextMeshProUGUI _lastName;
    public TextMeshProUGUI _email;
    public TextMeshProUGUI _gender;
    public TextMeshProUGUI _age;
    public TextMeshProUGUI _education;
    public TextMeshProUGUI _occupation;
    public TextMeshProUGUI _vehicle;
    public TextMeshProUGUI _frequency;

    [Header("Edit Profile Slot")]
    public TextMeshProUGUI _editfirstName;
    public TextMeshProUGUI _editlastName;
    public TextMeshProUGUI _editgender;
    public TextMeshProUGUI _editage;
    public TextMeshProUGUI _editeducation;
    public TextMeshProUGUI _editedoccupation;
    public TextMeshProUGUI _editvehicle;
    public TextMeshProUGUI _editfrequency;


    private void OnEnable()
    {
        if (_loginManager == null)
        {
            _loginManager = FindObjectOfType<LoginManager>();
        }

        //OnClickUpdateProfile();
        UpdateProfileLastest();
        /*_firstName.text = _loginManager._Account.first_name;
        _lastName.text = _loginManager._Account.last_name;
        _email.text = _loginManager._Account.email;
        _gender.text = _loginManager._Account.gender;
        _age.text = _loginManager._Account.age;
        _education.text = _loginManager._Account.education;
        _occupation.text = _loginManager._Account.occupation;
        _vehicle.text = _loginManager._Account.vehicle;
        _frequency.text = _loginManager._Account.checkpoint;*/
    }

    public GameObject _ProfileBtm;

    public void FixedUpdate()
    {
        if (_editfirstName.text.Length !=1&& _editlastName.text.Length !=1 && _editgender.text.Length != 1 && _editage.text.Length != 1 && _editeducation.text.Length != 1 && _editedoccupation.text.Length !=1)
        {
            _ProfileBtm.GetComponent<Button>().interactable = true;
        }
        else
        {
            _ProfileBtm.GetComponent<Button>().interactable = false;
        }

    }

    public void UpdateProfileLastest()
    {
        _firstName.text = _loginManager._Account.first_name;
        _lastName.text = _loginManager._Account.last_name;
        _email.text = _loginManager._Account.email;
        _gender.text = _loginManager._Account.gender;
        _age.text = _loginManager._Account.age;
        _education.text = _loginManager._Account.education;
        _occupation.text = _loginManager._Account.occupation;
        _vehicle.text = _loginManager._Account.vehicle;
        _frequency.text = _loginManager._Account.checkpoint;
    }
    public void OnClickUpdateProfile()
    {      
        StartCoroutine(UpdateProfiles());
    }

    public GameObject _LoadingPanel;
    public GameObject _OKPanel;
    public GameObject _FailedPanel;

    public GameObject _Edit_ProfileBTM;
    public GameObject _ProfilePanel;

    public PolicyScript _PolicyScript;

    [System.Serializable]
    public class _UpdateProfile
    {
        public string first_name;
        public string last_name;
        public string gender;
        public string age;
        public string education;
        public string occupation;
        public string vehicle;
        public string checkpoint;
    }

    public _UpdateProfile _Account;

    private IEnumerator UpdateProfiles()
    {
        Debug.Log("Click Updated Profile");

        _LoadingPanel.SetActive(true);

        _Account = new _UpdateProfile();

        _Account.first_name = _editfirstName.text;
        _Account.last_name = _editlastName.text;
        _Account.gender = _editgender.text;
        _Account.age = _editage.text;
        _Account.education = _editeducation.text;
        _Account.occupation = _editedoccupation.text;
        _Account.vehicle = _editvehicle.text;
        _Account.checkpoint = _editfrequency.text;

        string json = JsonUtility.ToJson(_Account);

        Debug.Log("BEFORE SENT : " + json);

        using var request = new UnityWebRequest(LoginManager.Instance._APIURL+ "/api/user/updateProfile", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", LoginManager.Instance._Account.access_token);

        yield return request.SendWebRequest();

        _LoadingPanel.SetActive(false);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("URL: " + request.url);

            _FailedPanel.SetActive(true);

        }
        else
        {
            Debug.Log("Update Response: " + request.downloadHandler.text);

            avatarResponse = JsonUtility.FromJson<ProfileResponse>(request.downloadHandler.text);

            if (avatarResponse.status)
            {
                Debug.Log("Update successful!");
                _loginManager._Account.first_name = avatarResponse.data.account.first_name;
                _loginManager._Account.last_name = avatarResponse.data.account.last_name;
                _loginManager._Account.gender = avatarResponse.data.account.gender;
                _loginManager._Account.age = avatarResponse.data.account.age;
                _loginManager._Account.education = avatarResponse.data.account.education;
                _loginManager._Account.occupation = avatarResponse.data.account.occupation;
                _loginManager._Account.vehicle = avatarResponse.data.account.vehicle;
                _loginManager._Account.checkpoint = avatarResponse.data.account.checkpoint;
                UpdateProfileLastest();
                //
            }
            else
            {
                Debug.LogError("Update failed!");
            }

            _OKPanel.SetActive(true);
            yield return new WaitForSeconds(3f);
            _OKPanel.SetActive(false);
            _Edit_ProfileBTM.SetActive(false);
           

            if (_PolicyScript._isForPdpa)
            {
                _PolicyScript.CheckforQuestionaire();
                _PolicyScript._isForPdpa = false;
            }
            else
            {
                _ProfilePanel.SetActive(true);
            }

            this.gameObject.SetActive(false);

        }


    }

    public ProfileResponse avatarResponse;


    [System.Serializable]
    public class Account
    {
        public string uid;
        public string email;
        public string password;
        public string facebook_id;
        public string google_id;
        public string first_name;
        public string last_name;
        public string gender;
        public string age;
        public string education;
        public string occupation;
        public string vehicle;
        public string checkpoint;
        public string platform;
        public string access_token;
        public int is_online;
        public string login_time;
        public string created_at;
    }
    [System.Serializable]
    public class ProfileResponse
    {
        public bool status;
        public Data data;
    }
    [System.Serializable]
    public class Data
    {
        public Account account;

    }
}
