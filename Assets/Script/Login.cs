using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.PunBasics;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }
    public TMP_InputField Email;
    public MaskedPasswordScript Password;
    public LaunCherTest1 _Launcher;
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

    public void OnClickLoginButton()
    {
        StartCoroutine(Login(Email.text, Password.actualInput));
    }

    private IEnumerator Login(string email, string password)
    {
        // สร้างอ็อบเจ็กต์การล็อกอิน
        var loginData = new LoginRequest
        {
            email = email,
            password = password
        };

        // แปลงอ็อบเจ็กต์การล็อกอินเป็น JSON
        string json = JsonUtility.ToJson(loginData);

        // สร้าง UnityWebRequest สำหรับ POST method
        using var request = new UnityWebRequest("http://13.250.106.216:1000/api/user/login", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        // ตั้งค่า header ของคำขอ
        request.SetRequestHeader("Content-Type", "application/json");

        // ส่งคำขอและรอรับการตอบกลับ
        yield return request.SendWebRequest();

        // ตรวจสอบผลลัพธ์ของคำขอ
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Login Response: " + request.downloadHandler.text);

            // แปลงข้อมูลตอบกลับเป็นอ็อบเจ็กต์
            var loginResponse = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);

            if (loginResponse.status)
            {
                Debug.Log("Login successful!");
                // ทำการเก็บข้อมูล token หรือข้อมูลอื่นๆ จากการล็อกอิน
                string uID = string.IsNullOrEmpty(loginResponse.data.account.uid) ? "-" : loginResponse.data.account.uid;
                string firstName = string.IsNullOrEmpty(loginResponse.data.account.first_name) ? "-" : loginResponse.data.account.first_name;
                string lastName = string.IsNullOrEmpty(loginResponse.data.account.last_name) ? "-" : loginResponse.data.account.last_name;
                string gender = string.IsNullOrEmpty(loginResponse.data.account.gender) ? "-" : loginResponse.data.account.gender;
                string age = string.IsNullOrEmpty(loginResponse.data.account.age) ? "-" : loginResponse.data.account.age;
                string education = string.IsNullOrEmpty(loginResponse.data.account.education) ? "-" : loginResponse.data.account.education;
                string occupation = string.IsNullOrEmpty(loginResponse.data.account.occupation) ? "-" : loginResponse.data.account.occupation;
                string vehicle = string.IsNullOrEmpty(loginResponse.data.account.vehicle) ? "-" : loginResponse.data.account.vehicle;
                string checkpoint = string.IsNullOrEmpty(loginResponse.data.account.checkpoint) ? "-" : loginResponse.data.account.checkpoint;
                string accesstoken = string.IsNullOrEmpty(loginResponse.data.account.access_token) ? "-" : loginResponse.data.account.access_token;

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
                _Avatar.accessory_ids = loginResponse.data.avatar.accessory_ids;


                if (_Avatar.name != null)
                {
                    Debug.Log("Found Avatar");
                    _Launcher.Connect();
                }
                else
                {
                    Debug.Log("No Avatar");
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
    }

    // โครงสร้างข้อมูลสำหรับผลลัพธ์การล็อกอิน
    [System.Serializable]
    public class LoginResponse
    {
        public bool status;
        public Data data;
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
        public int accessory_ids;
    }
}




