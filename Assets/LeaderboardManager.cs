using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
public class LeaderboardManager : MonoBehaviour
{
    public GameObject verticalLayout;
    private const string url = "https://api-motorway.mxrth.co:1000/api/leaderboard/getLeaderboard";
    private const float refreshInterval = 10f; // 5 minutes in seconds
    public LoginManager _loginManager;


    // Start is called before the first frame update
    void Start()
    {
        if (_loginManager != null)
        {
            Debug.Log("LoginManager is not null. Starting leaderboard data fetch.");
            StartCoroutine(GetLeaderboardData());

            // Schedule the GetLeaderboardData method to be called every 5 minutes
            Debug.Log("Scheduling leaderboard update every " + refreshInterval + " seconds.");
            InvokeRepeating("InvokeLeaderboardUpdate", refreshInterval, refreshInterval);
        }
        else
        {
            Debug.LogError("LoginManager is null. Cannot start leaderboard data fetch.");
        }
    }

    void Awake()
    {
        if (_loginManager == null)
        {
            _loginManager = FindObjectOfType<LoginManager>();
            if (_loginManager != null)
            {
                Debug.Log("LoginManager found in Awake.");
            }
            else
            {
                Debug.LogError("LoginManager not found in Awake.");
            }
        }
    }

    void InvokeLeaderboardUpdate()
    {
        Debug.Log("Invoking leaderboard update.");
        StartCoroutine(GetLeaderboardData());
    }

    [Serializable]
    public class LeaderboardResponse
    {
        public bool status;
        public Data data;
    }

    [Serializable]
    public class Data
    {
        public int total_page;
        public int page;
        public Leaderboard[] leaderboards;
        public User me;
    }

    [Serializable]
    public class Leaderboard
    {
        public string uid;
        public string account_uid;
        public int score;
        public string created_at; // Keep as string for parsing later
        public string email;
        public string first_name;
        public string last_name;
        public int rank;
    }

    [Serializable]
    public class User
    {
        public string uid;
        public string account_uid;
        public int score;
        public string created_at; // Keep as string for parsing later
        public int rank;
        public string email;
        public string first_name;
        public string last_name;
    }

    public LeaderboardResponse response;

    [Serializable]
    public class Pagination
    {
        public int page;
        public int size;
        public int sort;
    }

    IEnumerator GetLeaderboardData()
    {
        if (_loginManager == null)
        {
            _loginManager = FindObjectOfType<LoginManager>();
        }

        var pagination = new Pagination
        {
            page = 1,
            size = 10,
            sort = 1
        };

        string paginationJson = JsonUtility.ToJson(pagination);
        using var request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(paginationJson)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Authorization", _loginManager._Account.access_token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Request Error: " + request.error);
        }
        else
        {
            Debug.Log("Request successful. Processing data: " + request.downloadHandler.text);

            response = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
            JObject responseJson = JObject.Parse(request.downloadHandler.text);
            JArray leaderboards = (JArray)responseJson["data"]["leaderboards"];

            // Update existing Rank objects
            for (int i = 0; i < leaderboards.Count && i < verticalLayout.transform.childCount; i++)
            {
                JObject entry = (JObject)leaderboards[i];
                Transform rankTransform = verticalLayout.transform.GetChild(i);

                // Get references to the Num, Name, and Score components
                TextMeshProUGUI numText = rankTransform.Find("Num").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI nameText = rankTransform.Find("Name").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI scoreText = rankTransform.Find("Score").GetComponent<TextMeshProUGUI>();

                // Set the text values based on the leaderboard data
                numText.text = entry["rank"].ToString();
                nameText.text = entry["avatar_name"].ToString();
                scoreText.text = entry["score"].ToString();

                Debug.Log($"Updated Rank: {entry["rank"]}, Name: {entry["first_name"]}, Score: {entry["score"]}");
            }
        }
    }


}
