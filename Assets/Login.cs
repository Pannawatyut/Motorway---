using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;


public class LoginManager : MonoBehaviour
{
    public string email;
    public string password;

    public TextMeshProUGUI[] Infouser;

    private void Start()
    {
        StartCoroutine(Login(email, password));
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
                string firstName = string.IsNullOrEmpty(loginResponse.data.account.first_name) ? "-" : loginResponse.data.account.first_name;
                string lastName = string.IsNullOrEmpty(loginResponse.data.account.last_name) ? "-" : loginResponse.data.account.last_name;
                string gender = string.IsNullOrEmpty(loginResponse.data.account.gender) ? "-" : loginResponse.data.account.gender;
                string age = string.IsNullOrEmpty(loginResponse.data.account.age) ? "-" : loginResponse.data.account.age;
                string education = string.IsNullOrEmpty(loginResponse.data.account.education) ? "-" : loginResponse.data.account.education;
                string occupation = string.IsNullOrEmpty(loginResponse.data.account.occupation) ? "-" : loginResponse.data.account.occupation;
                string vehicle = string.IsNullOrEmpty(loginResponse.data.account.vehicle) ? "-" : loginResponse.data.account.vehicle;
                string checkpoint = string.IsNullOrEmpty(loginResponse.data.account.checkpoint) ? "-" : loginResponse.data.account.checkpoint;

                Infouser[0].text = "" + loginResponse.data.account.uid;
                Infouser[1].text = "" + loginResponse.data.account.email;
                Infouser[2].text = "" + firstName;
                Infouser[3].text = "" + lastName;
                Infouser[4].text = "" + gender;
                Infouser[5].text = "" + age;
                Infouser[6].text = "" + education;
                Infouser[7].text = "" + occupation;
                Infouser[8].text = "" + vehicle;
                Infouser[9].text = "" + checkpoint;
            }
            else
            {
                Debug.LogError("Login failed!");
            }
        }
    }
}

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
