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

    public GameObject _LoadingPanel;
    public GameObject _FailedPanel;
    public GameObject _OKPanel;
    public void OnClickRegisterButton()
    {
        StartCoroutine(Register(Email.text, Password.text));
    }

    private IEnumerator Register(string email, string password)
    {
        _LoadingPanel.SetActive(true);
        // สร้างอ็อบเจ็กต์การล็อกอิน
        var RegisterData = new RegisterInfo
        {
            email = email,
            password = password
        };

        // แปลงอ็อบเจ็กต์การล็อกอินเป็น JSON
        string json = JsonUtility.ToJson(RegisterData);

        // สร้าง UnityWebRequest สำหรับ POST method
        using var request = new UnityWebRequest(LoginManager.Instance._APIURL + "/api/user/register", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        // ตั้งค่า header ของคำขอ
        request.SetRequestHeader("Content-Type", "application/json");

        // ส่งคำขอและรอรับการตอบกลับ
        yield return request.SendWebRequest();
        _LoadingPanel.SetActive(false);
        // ตรวจสอบผลลัพธ์ของคำขอ
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            _FailedPanel.SetActive(true);
        }
        else
        {
            LoginPage.SetActive(true);
            RegisterPage.SetActive(false);
            _Register.email = email;
            _Register.password = password;
            Debug.Log("Login Response: " + request.downloadHandler.text);
            _OKPanel.SetActive(true);
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
