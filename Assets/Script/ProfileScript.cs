using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
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

    private IEnumerator UpdateProfiles()
    {
        Debug.Log("Click Updated Profile");
        var account = new Account
        {
            first_name = _editfirstName.text,
            last_name = _editlastName.text,
            gender = _editgender.text,
            age = _editage.text,
            education = _editeducation.text,
            occupation = _editedoccupation.text,
            vehicle = _editvehicle.text,
            checkpoint = _editfrequency.text
        };

        string json = JsonUtility.ToJson(account);

        using var request = new UnityWebRequest(LoginManager.Instance._APIURL+"/api/user/updateProfile", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", LoginManager.Instance._Account.access_token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("URL: " + request.url);
        }
        else
        {
            Debug.Log("Update Response: " + request.downloadHandler.text);
            var avatarResponse = JsonUtility.FromJson<ProfileResponse>(request.downloadHandler.text);

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
                this.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Update failed!");
            }
        }
    }
    [System.Serializable]
    public class Account
    {
        public string uid;
        public string email;
        public string first_name;
        public string last_name;
        public string gender;
        public string age;
        public string education;
        public string occupation;
        public string vehicle;
        public string checkpoint;
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
