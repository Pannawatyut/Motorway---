using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PlayGamesManager : MonoBehaviour
{
    void Awake()
    {
        // Configure Google Play Games
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestEmail()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        // Sign in at the start
        SignIn();
    }

    public void SignIn()
    {
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                Debug.Log("Signed in! Welcome " + Social.localUser.userName);
            }
            else
            {
                Debug.LogError("Failed to sign in.");
            }
        });
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        Debug.Log("User signed out.");
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            Debug.LogError("User not authenticated.");
        }
    }

    public void ShowAchievements()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
        else
        {
            Debug.LogError("User not authenticated.");
        }
    }
}
