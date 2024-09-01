using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class ResetPasswordScript : MonoBehaviour
{

    public GameObject LoginPage;
    public GameObject FirstPage;
    public GameObject SecondPage;

    [Header("FirstStep")]
    public TMP_InputField Email;


    [Header("SecondStep")]
    public string RefCode;
    public TextMeshProUGUI RefCodeText;
    public TMP_InputField OTP;
    public TMP_InputField Password;


    public GameObject _FailedPanel;
    public GameObject _FailedPanel_Email;
    public GameObject _LoadingPanel;
    public GameObject _OKPanel;

    #region FirstStep
    public void OnClickForgetPassword()
    {
        StartCoroutine(SendPasswordResetRequest(Email.text));
    }
    private IEnumerator SendPasswordResetRequest(string email)
    {
        _LoadingPanel.SetActive(true);
        // Create an instance of EmailData and assign the email
        EmailData emailData = new EmailData { email = email };

        // Convert the EmailData object to JSON
        string json = JsonUtility.ToJson(emailData);

        // Create a UnityWebRequest for the POST method
        using var request = new UnityWebRequest("http://13.250.106.216:1000/api/user/forgotPassword", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        // Set the request headers
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();
        _LoadingPanel.SetActive(false);
        // Check the result of the request
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            _FailedPanel_Email.SetActive(true);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // Convert the response to a PasswordResetResponse object
            var response = JsonUtility.FromJson<PasswordResetResponse>(request.downloadHandler.text);

            if (response.status)
            {
                Debug.Log("Password reset email sent successfully to: " + response.data.email);
                Debug.Log("Verification token: " + response.data.verify_token);
                FirstPage.SetActive(false);
                SecondPage.SetActive(true);
                RefCode = response.data.ref_code;
                RefCodeText.text = "RefCode: " + response.data.ref_code;
            }
            else
            {
                Debug.LogError("Failed to send password reset email.");
            }
        }
    }
    #endregion



    public void OnClickResetPassword()
    {
        StartCoroutine(SendResetPassword(Email.text));
    }
    private IEnumerator SendResetPassword(string email)
    {
        _LoadingPanel.SetActive(true);
        // Create an instance of EmailData and assign the email
        ResetPassword newPassword = new ResetPassword()
        {
            email = Email.text,
            password = Password.text,
            otp = OTP.text,
            ref_code = RefCode
        };

        // Convert the EmailData object to JSON
        string json = JsonUtility.ToJson(newPassword);

        // Create a UnityWebRequest for the POST method
        using var request = new UnityWebRequest("http://13.250.106.216:1000/api/user/resetPassword", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        // Set the request headers
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        _LoadingPanel.SetActive(false);
        // Check the result of the request
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            _FailedPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // Convert the response to a PasswordResetResponse object
            var response = JsonUtility.FromJson<PasswordResetResponse>(request.downloadHandler.text);

            if (response.status)
            {
                Debug.Log("Password Successfully change to : " + response.data.password);
                // LoginPage.SetActive(true);
                // SecondPage.SetActive(false);
                _OKPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to send password reset email.");
            }
        }
    }

    [System.Serializable]
    public class EmailData
    {
        public string email;
    }
    [System.Serializable]
    public class ResetPassword
    {
        public string email;       
        public string password;
        public string otp;
        public string ref_code;
    }

    // Class representing the response from the server
    [System.Serializable]
    public class PasswordResetResponse
    {
        public bool status;
        public Data data;

        [System.Serializable]
        public class Data
        {
            public string email;
            public string verify_token;
            public string otp;
            public string password;
            public string ref_code;
        }
    }
}

