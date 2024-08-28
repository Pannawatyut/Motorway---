using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Facebook.Unity;
using System;

public class FacebookManager : MonoBehaviour
{
    #region Initialize

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
        var permissions = new List<string> { "public_profile" };
        FB.LogInWithReadPermissions(permissions, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Login successful.");
            LogAccessTokenInfo();
            DealWithFbMenus(true);
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

    #region Share (Optional, if needed in the future)

    // Uncomment if needed
    /*
    public void FacebookSharefeed()
    {
        string url = "https://developers.facebook.com/docs/unity/reference/current/FB.ShareLink";
        FB.ShareLink(
            new Uri(url),
            "Checkout COCO 3D channel",
            "I just watched " + "22" + " times of this channel",
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
