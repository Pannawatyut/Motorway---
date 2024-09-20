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
            //OnSubmitQuestionaire();
        }
    }

    public void OnSubmitQuestionaire()
    {
        StartCoroutine(SubmitQuestionaire());
    }

    public string _Test;

    public GameObject _QAPanel;

    public GameObject _LoadingBar;
    public GameObject _LoadingFailed;
    public GameObject _LoadingOK;

    public GameObject _PolicyGameObject;
    public GameObject _QuesitonaireObject;

    public GameObject _ChatBoxObject;

    public TextMeshProUGUI _gender;
    public TextMeshProUGUI _age;
    public TextMeshProUGUI _education;
    public TextMeshProUGUI _occupation;
    public TextMeshProUGUI _social_media;
    public TextMeshProUGUI _vehicle_type;

    IEnumerator SubmitQuestionaire()
    {
        _LoadingBar.SetActive(true);
        // Initialize SurveyData object
        _questionaire = new SurveyData();
        _questionaire.gender = _gender.text;
        _questionaire.age = _age.text;
        _questionaire.education = _education.text;
        _questionaire.occupation = _occupation.text;
        _questionaire.social_media = _social_media.text;
        _questionaire.vehicle_type = _vehicle_type.text;

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
        _LoadingBar.SetActive(false);

        string json = JsonUtility.ToJson(_questionaire);
        Debug.Log(json);
        using var request = new UnityWebRequest(LoginManager.Instance._APIURL+ "/api/questionnaire/createQuestionnaire", "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", _loginManager._Account.access_token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            _LoadingOK.SetActive(true);
            yield return new WaitForSeconds(3f);
            _LoadingOK.SetActive(false);
            _QAPanel.SetActive(false);
            
            //cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ButtonChangePlayerCanMove.Reset = false;
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("URL: " + request.url);
        }
        else
        {

            _LoadingOK.SetActive(true);
            yield return new WaitForSeconds(3f);
            _LoadingOK.SetActive(false);
            _QAPanel.SetActive(false);
            //cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ButtonChangePlayerCanMove.Reset = false;

            LoginManager.Instance._Account.is_questionnaire = 1;
            Debug.Log("SubmitQuestionaire Response: " + request.downloadHandler.text);
        }

        if(_PolicyGameObject)
            _PolicyGameObject.SetActive(false);
        if (_QuesitonaireObject)
            _QuesitonaireObject.SetActive(false);
        if (_ChatBoxObject)
            _ChatBoxObject.SetActive(true);

        CursorManagerScript.Instance.DisableCursor();
    }

    public SurveyData _questionaire;
    [System.Serializable]
    public class SurveyData
    {
        public string gender;
        public string age;
        public string education;
        public string occupation;
        public string social_media;
        public string vehicle_type;
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
