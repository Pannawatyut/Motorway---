using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FacebookManager : MonoBehaviour
{
    #region Initialize
    [Serializable]
    public class UserData
    {
        public string email;
        public string facebook_id;
        public string platform;
    }

    public LoginManager loginManager;

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
                FetchProfile(); // Fetch email and Facebook ID
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
            FetchProfile(); // Fetch email and Facebook ID
        }
        else
        {
            Debug.LogError("Login failed: " + result.Error);
            loginManager._ErrorMessage.text = result.Error;
            loginManager._LoadingFailed.SetActive(true);
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

        // Send email, Facebook ID, and platform
        StartCoroutine(SendEmailandFacebookID(email, facebookId));
    }

    private string GetPlatform()
    {
#if UNITY_WEBGL
        return "WEBGL";
#elif UNITY_ANDROID
        return "ANDROID";
#elif UNITY_IOS
        return "IOS";
#else
        return "UNKNOWN";
#endif
    }

    private IEnumerator SendEmailandFacebookID(string email, string facebookId)
    {
        string platform = GetPlatform();

        // Create an instance of UserData and assign the email, Facebook ID, and platform
        UserData userData = new UserData
        {
            email = email,
            facebook_id = facebookId,
            platform = platform,
        };

        // Convert the UserData object to JSON
        string json = JsonUtility.ToJson(userData);

        // Create a UnityWebRequest for the POST method
        using var request = new UnityWebRequest(
            "https://api-motorway.mxrth.co:1000/api/user/loginFacebook",
            "POST"
        )
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer(),
        };
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        loginManager._LoadingBar.SetActive(false);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            loginManager._ErrorMessage.text = request.error;
            loginManager._LoadingFailed.SetActive(true);

            var loginResponse = JsonUtility.FromJson<LoginManager.LoginResponse>(request.downloadHandler.text);
            Debug.LogError("Error Type: " + loginResponse.error.message);
            loginManager._ErrorMessage.text = loginResponse.error.message;

        }
        else
        {
            Debug.Log("Login Response: " + request.downloadHandler.text);
            loginManager._LoadingOK.SetActive(true);

            // Process login response (as done previously)
            var loginResponse = JsonUtility.FromJson<LoginManager.LoginResponse>(request.downloadHandler.text);

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
                loginManager._Account.uid = uID;
                loginManager._Account.first_name = firstName;
                loginManager._Account.last_name = lastName;
                loginManager._Account.email = loginResponse.data.account.email;
                loginManager._Account.gender = gender;
                loginManager._Account.age = age;
                loginManager._Account.education = education;
                loginManager._Account.occupation = occupation;
                loginManager._Account.vehicle = vehicle;
                loginManager._Account.checkpoint = checkpoint;
                loginManager._Account.access_token = accesstoken;

                loginManager._Avatar.uid = loginResponse.data.avatar.uid;
                loginManager._Avatar.name = loginResponse.data.avatar.name;
                loginManager._Avatar.gender_id = loginResponse.data.avatar.gender_id;
                loginManager._Avatar.skin_id = loginResponse.data.avatar.skin_id;
                loginManager._Avatar.face_id = loginResponse.data.avatar.face_id;
                loginManager._Avatar.hair_id = loginResponse.data.avatar.hair_id;
                loginManager._Avatar.hair_color_id = loginResponse.data.avatar.hair_color_id;
                loginManager._Avatar.shirt_id = loginResponse.data.avatar.shirt_id;
                loginManager._Avatar.shirt_color_id = loginResponse.data.avatar.shirt_color_id;
                loginManager._Avatar.pant_id = loginResponse.data.avatar.pant_id;
                loginManager._Avatar.pant_color_id = loginResponse.data.avatar.pant_color_id;
                loginManager._Avatar.shoe_id = loginResponse.data.avatar.shoe_id;
                loginManager._Avatar.shoe_color_id = loginResponse.data.avatar.shoe_color_id;
                loginManager._Avatar.accessory_id = loginResponse.data.avatar.accessory_id;

                if (loginManager._Avatar.name != null)
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
