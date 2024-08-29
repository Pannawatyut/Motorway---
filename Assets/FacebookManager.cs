using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Facebook.Unity;
using System;
using UnityEngine.Networking;

public class FacebookManager : MonoBehaviour
{

    #region Initialize
    [Serializable]
    public class UserData
    {
        public string email;
        public string facebook_id;
    }

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Facebook SDK initialized successfully.");
            if (FB.IsLoggedIn)
            {
                Debug.Log("User is already logged in.");
                LogAccessTokenInfo();
                FetchProfile();  // Fetch email and Facebook ID
            }
            else
            {
                Debug.Log("User is not logged in.");
            }
        }
        else
        {
            Debug.LogError("Failed to initialize the Facebook SDK.");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        Time.timeScale = isGameShown ? 1 : 0;
    }

    private void LogAccessTokenInfo()
    {
        if (FB.IsLoggedIn)
        {
            var token = AccessToken.CurrentAccessToken;
            Debug.Log("User ID: " + token.UserId);
            Debug.Log("Access Token: " + token.TokenString);
            Debug.Log("Permissions: " + string.Join(", ", token.Permissions));
        }
    }

    private void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            Debug.Log("Facebook is logged in.");
        }
        else
        {
            Debug.Log("Facebook is not logged in.");
        }
    }

    #endregion

    #region Login / Logout

    // Login
    public void Facebook_LogIn()
    {
        var permissions = new List<string> { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Login successful.");
            LogAccessTokenInfo();
            DealWithFbMenus(true);
            FetchProfile();  // Fetch email and Facebook ID
        }
        else
        {
            Debug.LogError("Login failed: " + result.Error);
        }
    }

    // Logout
    public void Facebook_LogOut()
    {
        StartCoroutine(LogOutCoroutine());
    }

    private IEnumerator LogOutCoroutine()
    {
        FB.LogOut();
        while (FB.IsLoggedIn)
        {
            Debug.Log("Logging out...");
            yield return null;
        }
        Debug.Log("Logout successful.");
    }

    #endregion

    #region User Profile

    private void FetchProfile()
    {
        FB.API("/me?fields=id,name,email", HttpMethod.GET, ProfileCallback);
    }

    private void ProfileCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError("Error fetching profile: " + result.Error);
            return;
        }

        var profile = result.ResultDictionary;
        string email = string.Empty;
        string facebookId = string.Empty;

        if (profile.ContainsKey("id"))
        {
            facebookId = profile["id"].ToString();
            Debug.Log("Facebook ID: " + facebookId);
        }
        if (profile.ContainsKey("email"))
        {
            email = profile["email"].ToString();
            Debug.Log("Email: " + email);
        }

        // Start sending email and Facebook ID to backend
        StartCoroutine(SendEmailandFacebookID(email, facebookId));
    }

    private IEnumerator SendEmailandFacebookID(string email, string facebookId)
    {
        // Create an instance of UserData and assign the email and Facebook ID
        UserData userData = new UserData { email = email, facebook_id = facebookId };

        // Convert the UserData object to JSON
        string json = JsonUtility.ToJson(userData);

        // Create a UnityWebRequest for the POST method
        using var request = new UnityWebRequest("http://192.168.1.214:7000/api/user/loginFacebook", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Successfully sent email and Facebook ID to backend.");
        }
        else
        {
            Debug.LogError("Failed to send email and Facebook ID to backend: " + request.error);
        }
    }


    #endregion

    #region Share (Optional, if needed in the future)

    // Uncomment if needed
    /*
    public void FacebookSharefeed()
    {
        string url = "https://developers.facebook.com/docs/unity/reference/current/FB.ShareLink";
        FB.ShareLink(
            new Uri(url),
            null,
            ShareCallback);
    }

    private static void ShareCallback(IShareResult result)
    {
        Debug.Log("ShareCallback");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
        Debug.Log(result.RawResult);
    }
    */

    #endregion
}
