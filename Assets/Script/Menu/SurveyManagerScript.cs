using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SurveyManagerScript : MonoBehaviour
{
    public SurveyScript[] _score;
    public LoginManager _loginManager;
    public TextMeshProUGUI _suggestion;
    private void Update()
    {
        if (_loginManager == null)
        {
            _loginManager = FindObjectOfType<LoginManager>();
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            OnSubmitQuestionaire();
        }
    }

    public void OnSubmitQuestionaire()
    {
        StartCoroutine(SubmitQuestionaire());
    }

    public string _Test;

    IEnumerator SubmitQuestionaire()
    {
        // Initialize SurveyData object
        _questionaire = new SurveyData();
        _questionaire.suggestion = _suggestion.text;

        // Initialize the questions list to avoid NullReferenceException
        _questionaire.questions = new List<Question>();

        // Loop through the score array to create and add questions
        for (int i = 0; i < _score.Length; i++)
        {
            Question x = new Question
            {
                answer_id = _score[i].Score.ToString(),
                question_id = i + 1, // Assuming the question_id is just the index + 1
                question_text = "Question Number" + i // Set your question text here
            };

            // Add the question to the list
            _questionaire.questions.Add(x);
        }

        yield return null;

        string json = JsonUtility.ToJson(_questionaire);
        Debug.Log(json);
        using var request = new UnityWebRequest("http://13.250.106.216:1000/api/questionnaire/createQuestionnaire", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", _loginManager._Account.access_token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("URL: " + request.url);
        }
        else
        {
            Debug.Log("Login Response: " + request.downloadHandler.text);
        }
    }

    public SurveyData _questionaire;
    [System.Serializable]
    public class SurveyData
    {
        public List<Question> questions;
        public string suggestion;
    }

    [System.Serializable]
    public class Question
    {
        public int question_id;
        public string question_text;
        public string answer_id;
    }
}
