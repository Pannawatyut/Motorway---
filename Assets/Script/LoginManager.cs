using System;
using System.Collections;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }
    public TMP_InputField Email;
    public TMP_InputField Password;
    public LaunCherTest1 _Launcher;
    public LoginButtonScript _LoginButtonScript;
    public GameObject _LoadingBar;
    public GameObject _LoadingFailed;
    public GameObject _LoadingOK;
    public string platform;
    public string _APIURL;

    public bool rememberPassword = true; // Toggle this based on user selection from the UI
    public bool _isStarter; // Already Show Welcome Panel once -> true
    public TextMeshProUGUI _ErrorMessage;

    private void Awake()
 {
     if (Instance == null)
     {
         Instance = this;
     }
     else
     {
         Destroy(gameObject);
         return;
     }
     DontDestroyOnLoad(gameObject);

     //Test();
 }

 private void OnEnable()
 {
     if (_LoginButtonScript == null)
     {
         _LoginButtonScript = FindAnyObjectByType<LoginButtonScript>();
         if (_LoginButtonScript != null)
         {
             Email = _LoginButtonScript._Email;
             //Password = _LoginButtonScript.pa;
             _LoadingBar = _LoginButtonScript._loading;
             _LoadingFailed = _LoginButtonScript._loadingFailed;
             _LoadingOK = _LoginButtonScript._loadingOk;
             //_ErrorMessage = _LoginButtonScript._errorMessage;
         }
     }
 }
 
 private void Start()
 {
     if (PlayerPrefs.GetInt("RememberPassword") == 1)
     {
         rememberPassword = true;
         _isCheck.isOn = true;
     }
     else
     {
         rememberPassword = false;
         _isCheck.isOn = false;
     }

     if (rememberPassword)
     {
         string savedEmail = PlayerPrefs.GetString("SavedEmail", "");
         string savedPassword = PlayerPrefs.GetString("SavedPassword", "");

         Email.text = savedEmail;
         Password.GetComponent<TMP_InputField>().text = savedPassword;
         //Password.actualInput = savedPassword;
     }
     
 }

 public Toggle _isCheck;
 public void _isRemember()
 {
     if (_isCheck.isOn)
     {
         PlayerPrefs.SetInt("RememberPassword", 1);
         rememberPassword = true; // Set the toggle based on saved preference
     }
     else
     {
         PlayerPrefs.SetInt("RememberPassword", 0);
         rememberPassword = false;
     }
 }
    public void OnClickLoginButton()
    {
#if UNITY_WEBGL
        platform = "WEBGL";
#elif UNITY_ANDROID
        platform = "ANDROID";
#elif UNITY_IOS
        platform = "IOS";
#else
        platform = "UNKNOWN";
#endif
        StartCoroutine(Login(Email.text, Password.text, platform));
    }

    public void _ByPass()
    {
        StartCoroutine(Login("pongsakorn.pisa@kmutt.ac.th", "123", "TEST"));
    }

    public IEnumerator _GoogleLoginAPI(string _Email, string google_id, string platform)
    {
        Debug.Log(
            $"_GoogleLoginAPI called with Email: {_Email}, Google ID: {google_id}, Platform: {platform}"
        );

        // Show loading bar while the request is processed
        _LoadingBar.SetActive(true);

        // Prepare the object to send, now including the platform
        _ThirdPartyData_Google Obj = new _ThirdPartyData_Google
        {
            email = _Email,
            google_id = google_id,
            platform = platform,
        };

        // Serialize object to JSON
        string json = JsonUtility.ToJson(Obj);
        Debug.Log("SENT GOOGLE API: " + json);

        // Setup the request
        var request = new UnityWebRequest(
            "https://api-motorway.mxrth.co:1000/api/user/loginGoogle",
            "POST"
        );
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("Sending Google Login API request...");

        // Send the request
        yield return request.SendWebRequest();

        _LoadingBar.SetActive(false);
        
        Debug.Log("Request sent, awaiting response...");

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            _LoadingBar.SetActive(false);
            _LoadingFailed.SetActive(true);
            var loginResponse = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            Debug.LogError("Error Type: " + loginResponse.error.message);
            _ErrorMessage.text = loginResponse.error.message;
            yield break;
        }
        else
        {
             // Debug info for response
        Debug.Log("request responseCode: " + request.responseCode);
        Debug.Log("request responseText: " + request.downloadHandler.text);

        // Deserialize the response
        LoginResponse loginResponse;
        try
        {
            loginResponse = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            Debug.Log("LoginResponse: " + request.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse login response: " + e.Message);
            _LoadingBar.SetActive(false);
            _LoadingFailed.SetActive(true);
            _ErrorMessage.text = e.Message;
            yield break;
        }
        Debug.Log($"Sending JSON to API: {json}");

        // Handle successful login
        if (loginResponse.status)
        {
            Debug.Log("Login successful!");

            // Update account data
            _Account.uid = string.IsNullOrEmpty(loginResponse.data.account.uid)
                ? "-"
                : loginResponse.data.account.uid;
            _Account.first_name = string.IsNullOrEmpty(loginResponse.data.account.first_name)
                ? "-"
                : loginResponse.data.account.first_name;
            _Account.last_name = string.IsNullOrEmpty(loginResponse.data.account.last_name)
                ? "-"
                : loginResponse.data.account.last_name;
            _Account.email = loginResponse.data.account.email;
            _Account.gender = string.IsNullOrEmpty(loginResponse.data.account.gender)
                ? "-"
                : loginResponse.data.account.gender;
            _Account.age = string.IsNullOrEmpty(loginResponse.data.account.age)
                ? "-"
                : loginResponse.data.account.age;
            _Account.education = string.IsNullOrEmpty(loginResponse.data.account.education)
                ? "-"
                : loginResponse.data.account.education;
            _Account.occupation = string.IsNullOrEmpty(loginResponse.data.account.occupation)
                ? "-"
                : loginResponse.data.account.occupation;
            _Account.vehicle = string.IsNullOrEmpty(loginResponse.data.account.vehicle)
                ? "-"
                : loginResponse.data.account.vehicle;
            _Account.checkpoint = string.IsNullOrEmpty(loginResponse.data.account.checkpoint)
                ? "-"
                : loginResponse.data.account.checkpoint;
            _Account.access_token = string.IsNullOrEmpty(loginResponse.data.account.access_token)
                ? "-"
                : loginResponse.data.account.access_token;

            // Update avatar data
            _Avatar.uid = loginResponse.data.avatar.uid;
            _Avatar.name = loginResponse.data.avatar.name;
            _Avatar.gender_id = loginResponse.data.avatar.gender_id;
            _Avatar.skin_id = loginResponse.data.avatar.skin_id;
            _Avatar.face_id = loginResponse.data.avatar.face_id;
            _Avatar.hair_id = loginResponse.data.avatar.hair_id;
            _Avatar.hair_color_id = loginResponse.data.avatar.hair_color_id;
            _Avatar.shirt_id = loginResponse.data.avatar.shirt_id;
            _Avatar.shirt_color_id = loginResponse.data.avatar.shirt_color_id;
            _Avatar.pant_id = loginResponse.data.avatar.pant_id;
            _Avatar.pant_color_id = loginResponse.data.avatar.pant_color_id;
            _Avatar.shoe_id = loginResponse.data.avatar.shoe_id;
            _Avatar.shoe_color_id = loginResponse.data.avatar.shoe_color_id;
            _Avatar.accessory_id = loginResponse.data.avatar.accessory_id;

            // Navigate based on avatar existence
            if (!string.IsNullOrEmpty(_Avatar.name))
            {
                Debug.Log("Found Avatar");
                yield return new WaitForSeconds(3f);
                SceneManager.LoadScene("Game");
            }
            else
            {
                Debug.Log("No Avatar");
                yield return new WaitForSeconds(3f);
                SceneManager.LoadScene("CharacterCustomizer");
            }

            _LoadingOK.SetActive(true);
        }
        else
        {
            Debug.LogError("Login failed!");
            _LoadingFailed.SetActive(true);
        }

        }

        // Hide loading bar
        _LoadingBar.SetActive(false);
    }

    private IEnumerator Login(string email, string password, string platform)
    {
        // สร้างอ็อบเจ็กต์การล็อกอิน
        _LoadingBar.SetActive(true);
        var loginData = new LoginRequest
        {
            email = email,
            password = password,
            platform = platform,
        };

        // แปลงอ็อบเจ็กต์การล็อกอินเป็น JSON
        string json = JsonUtility.ToJson(loginData);

        // สร้าง UnityWebRequest สำหรับ POST method
        using var request = new UnityWebRequest(
            LoginManager.Instance._APIURL + "/api/user/login",
            "POST"
        )
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer(),
        };

        // ตั้งค่า header ของคำขอ
        request.SetRequestHeader("Content-Type", "application/json");

        // ส่งคำขอและรอรับการตอบกลับ
        yield return request.SendWebRequest();
        _LoadingBar.SetActive(false);
        // ตรวจสอบผลลัพธ์ของคำขอ
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            _LoadingFailed.SetActive(true);
            var loginResponse = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            Debug.LogError("Error Type: " + loginResponse.error.message);
            _ErrorMessage.text = loginResponse.error.message;

        }
        else
        {

            if (rememberPassword)
            {
                PlayerPrefs.SetString("SavedEmail", email);                   
                PlayerPrefs.SetString("SavedPassword", password);
                PlayerPrefs.SetInt("RememberPassword", 1); // Store flag for remembering
                Debug.Log("Remembered Password: " + password);
            }
            else
            {
                PlayerPrefs.DeleteKey("SavedEmail");
                PlayerPrefs.DeleteKey("SavedPassword");
                PlayerPrefs.SetInt("RememberPassword", 0); // Clear the flag if unchecked
            }
            
            Debug.Log("Login Response: " + request.downloadHandler.text);

            _LoadingOK.SetActive(true);
            // แปลงข้อมูลตอบกลับเป็นอ็อบเจ็กต์
            var loginResponse = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if (loginResponse.status)
            {
                Debug.Log("Login successful!");
                // ทำการเก็บข้อมูล token หรือข้อมูลอื่นๆ จากการล็อกอิน
                string uID = string.IsNullOrEmpty(loginResponse.data.account.uid)
                    ? "-"
                    : loginResponse.data.account.uid;
                string firstName = string.IsNullOrEmpty(loginResponse.data.account.first_name)
                    ? "-"
                    : loginResponse.data.account.first_name;
                string lastName = string.IsNullOrEmpty(loginResponse.data.account.last_name)
                    ? "-"
                    : loginResponse.data.account.last_name;
                string gender = string.IsNullOrEmpty(loginResponse.data.account.gender)
                    ? "-"
                    : loginResponse.data.account.gender;
                string age = string.IsNullOrEmpty(loginResponse.data.account.age)
                    ? "-"
                    : loginResponse.data.account.age;
                string education = string.IsNullOrEmpty(loginResponse.data.account.education)
                    ? "-"
                    : loginResponse.data.account.education;
                string occupation = string.IsNullOrEmpty(loginResponse.data.account.occupation)
                    ? "-"
                    : loginResponse.data.account.occupation;
                string vehicle = string.IsNullOrEmpty(loginResponse.data.account.vehicle)
                    ? "-"
                    : loginResponse.data.account.vehicle;
                string checkpoint = string.IsNullOrEmpty(loginResponse.data.account.checkpoint)
                    ? "-"
                    : loginResponse.data.account.checkpoint;
                string accesstoken = string.IsNullOrEmpty(loginResponse.data.account.access_token)
                    ? "-"
                    : loginResponse.data.account.access_token;

                _Account.uid = uID;
                _Account.first_name = firstName;
                _Account.last_name = lastName;
                _Account.email = loginResponse.data.account.email;
                _Account.gender = gender;
                _Account.age = age;
                _Account.education = education;
                _Account.occupation = occupation;
                _Account.vehicle = vehicle;
                _Account.checkpoint = checkpoint;
                _Account.access_token = accesstoken;
                _Account.is_questionnaire = loginResponse.data.account.is_questionnaire;

                _Avatar.uid = loginResponse.data.avatar.uid;
                _Avatar.name = loginResponse.data.avatar.name;
                _Avatar.gender_id = loginResponse.data.avatar.gender_id;
                _Avatar.skin_id = loginResponse.data.avatar.skin_id;
                _Avatar.face_id = loginResponse.data.avatar.face_id;
                _Avatar.hair_id = loginResponse.data.avatar.hair_id;
                _Avatar.hair_color_id = loginResponse.data.avatar.hair_color_id;
                _Avatar.shirt_id = loginResponse.data.avatar.shirt_id;
                _Avatar.shirt_color_id = loginResponse.data.avatar.shirt_color_id;
                _Avatar.pant_id = loginResponse.data.avatar.pant_id;
                _Avatar.pant_color_id = loginResponse.data.avatar.pant_color_id;
                _Avatar.shoe_id = loginResponse.data.avatar.shoe_id;
                _Avatar.shoe_color_id = loginResponse.data.avatar.shoe_color_id;
                _Avatar.accessory_id = loginResponse.data.avatar.accessory_id;

                if (_Avatar.name != null)
                {
                    Debug.Log("Found Avatar");
                    yield return new WaitForSeconds(3f);
                    SceneManager.LoadScene("Game");
                }
                else
                {
                    Debug.Log("No Avatar");
                    yield return new WaitForSeconds(3f);
                    SceneManager.LoadScene("CharacterCustomizer");
                }
            }
            else
            {
                Debug.LogError("Login failed!");
            }
        }
    }

    public Account _Account;
    public Avatar _Avatar;

    // โครงสร้างข้อมูลสำหรับคำขอล็อกอิน
[System.Serializable]
public class LoginRequest
{
    public string email;
    public string password;
    public string platform;
}

// โครงสร้างข้อมูลสำหรับผลลัพธ์การล็อกอิน
[System.Serializable]
public class LoginResponse
{
    public bool status;
    public Error error;
    public Data data;
}
[System.Serializable]
public class Error
{
    public int error_code;
    public string message;
}

[System.Serializable]
public class Data
{
    public Account account;
    public Avatar avatar;
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
    public string access_token;
    public int is_questionnaire;
}

public class _ThirdPartyData_Google
{
    public string email;
    public string google_id;
    public string platform;
}

[System.Serializable]
public class Avatar
{
    public string uid;
    public string name;
    public int gender_id;
    public int skin_id;
    public int face_id;
    public int hair_id;
    public int hair_color_id;
    public int shirt_id;
    public int shirt_color_id;
    public int pant_id;
    public int pant_color_id;
    public int shoe_id;
    public int shoe_color_id;
    public int accessory_id;
}
}
