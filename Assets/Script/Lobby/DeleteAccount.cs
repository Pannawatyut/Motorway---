using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class DeleteAccount : MonoBehaviour
{
    public LoginManager _loginManager;

    private void Update()
    {
        if (_loginManager == null)
        {
            _loginManager = FindObjectOfType<LoginManager>();
        }
    }
    public void OnClickDeleteAccount()
    {
        StartCoroutine(DeleteAccounts());
    }

    IEnumerator DeleteAccounts()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(LoginManager.Instance._APIURL + "/api/user/deleteAccount"))
        {
            // Add the necessary headers
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", _loginManager._Account.access_token);

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            // Check if the request succeeded or failed
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to Delete Account: " + request.error);
            }
            else
            {
                Debug.Log("DeleteAccount Response: " + request.downloadHandler.text);
                SceneManager.LoadScene(0);
                
            }

        }
    }
}
