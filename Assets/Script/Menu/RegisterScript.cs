using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;



public class RegisterScript : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField Password;

    public GameObject LoginPage;
    public GameObject RegisterPage;
    public void OnClickRegisterButton()
    {
        StartCoroutine(Register(Email.text, Password.text));
    }

    private IEnumerator Register(string email, string password)
    {
        // สร้างอ็อบเจ็กต์การล็อกอิน
        var RegisterData = new RegisterInfo
        {
            email = email,
            password = password
        };

        // แปลงอ็อบเจ็กต์การล็อกอินเป็น JSON
        string json = JsonUtility.ToJson(RegisterData);

        // สร้าง UnityWebRequest สำหรับ POST method
        using var request = new UnityWebRequest("http://13.250.106.216:1000/api/user/register", "POST")
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
            LoginPage.SetActive(true);
            RegisterPage.SetActive(false);
            _Register.email = email;
            _Register.password = password;
            Debug.Log("Login Response: " + request.downloadHandler.text);
        }
    }
    public RegisterInfo _Register;

    [System.Serializable]
    public class RegisterInfo
    {
        public string email;
        public string password;
    }
}
