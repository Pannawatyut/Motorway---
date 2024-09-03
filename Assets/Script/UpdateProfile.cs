using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateProfile : MonoBehaviour
{
    public string firstName = "sirichai";
    public string lastName = "Tang";
    public string gender = "ชาย";
    public string age = "20-30 ปี";
    public string education = "ปริญาเอก";
    public string occupation = "นักศึกษา";
    public string vehicle = "รถยนต์ส่วนตัว";
    public string checkpoint = "ทุกวัน";

    public void OnUpdateButtonClicked()
    {
        StartCoroutine(UpdateUser());
    }

    private IEnumerator UpdateUser()
    {
        var updateData = new UpdateUserRequest
        {
            account = new AccountUpdate
            {
                first_name = firstName,
                last_name = lastName,
                gender = gender,
                age = age,
                education = education,
                occupation = occupation,
                vehicle = vehicle,
                checkpoint = checkpoint
            }
        };

        string json = JsonUtility.ToJson(updateData);

        // ตรวจสอบ JSON payload ที่จะส่ง
        Debug.Log("JSON Payload: " + json);

        using var request = new UnityWebRequest(LoginManager.Instance._APIURL + "/api/user/updateProfile", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

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
            Debug.Log("Response Code: " + request.responseCode);
            Debug.Log("Response Text: " + request.downloadHandler.text);

            // แสดงผลลัพธ์ในกรณีที่คำขอสำเร็จ
            if (request.responseCode == 200)
            {
                Debug.Log("Update successful!");
            }
            else
            {
                Debug.LogError("Update failed with response code: " + request.responseCode);
            }
        }
    }
}

[System.Serializable]
public class UpdateUserRequest
{
    public AccountUpdate account;
}

[System.Serializable]
public class AccountUpdate
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