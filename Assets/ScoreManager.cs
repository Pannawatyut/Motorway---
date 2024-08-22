using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public float time = 180f; // 3 minutes in seconds
    public int score = 0;

    public int FirstTimePlay;

    public int ScoreEvaluation = 0;
    public TMP_Text timerText; // TextMeshPro component to display the timer
    public TMP_Text scoreText; // TextMeshPro component to display the score
    public TMP_Text end_score;

    public TMP_Text Combo;

    public TMP_Text MultiplierScore;

    public GameObject[] Combotext;

    public GameObject End_Menu;

    public GameObject End_Menu2;
    public GameObject End_Menu3;

    public GameObject[] ImageEmoji;

    public GameObject Setting;

    public static ScoreManager Instance { get; private set; }

    public int consecutiveCorrectAnswers = 0;
    public int scoreMultiplier = 1;

    private void Awake()
    {
        // Ensure that there is only one instance of ScoreManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(CountdownTimer());
        UpdateScoreDisplay(); // Initialize the score display
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = score.ToString();
    }

    private IEnumerator CountdownTimer()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }
        time = 0; // Ensure time doesn't go negative
        UpdateTimerDisplay();
        // Trigger an event or method when time runs out
        OnTimerEnd();
    }

    public void OnTimerEnd()
    {
        time = 0;
        // Implement what should happen when the timer ends
        Debug.Log("Timer ended");
        End_Menu.SetActive(true);
        end_score.text = score.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount * scoreMultiplier;
        UpdateScoreDisplay(); // Update the score display
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0)
        {
            score = 0;
        }
        UpdateScoreDisplay(); // Update the score display
    }

    public void CorrectAnswer()
    {
        consecutiveCorrectAnswers++;
        Combo.text = consecutiveCorrectAnswers.ToString();
        if (consecutiveCorrectAnswers > 0)
        {
            Combotext[0].SetActive(true);
            Combotext[1].SetActive(true);
        }
        if (consecutiveCorrectAnswers == 0)
        {
            Combotext[0].SetActive(false);
            Combotext[1].SetActive(false);
        }
        UpdateMultiplier();
        StartCoroutine(ScaleText(Combo, 1.5f, 0.2f)); // Call the scaling animation
    }

    public void WrongAnswer()
    {
        consecutiveCorrectAnswers = 0;
        Combo.text = "";
        MultiplierScore.text = "";
        Combotext[0].SetActive(false);
        Combotext[1].SetActive(false);
        scoreMultiplier = 1;
    }

    private void UpdateMultiplier()
    {
        if (consecutiveCorrectAnswers >= 10)
        {
            scoreMultiplier = 3;
            MultiplierScore.text = "X" + scoreMultiplier;
        }
        else if (consecutiveCorrectAnswers >= 5)
        {
            scoreMultiplier = 2;
            MultiplierScore.text = "X" + scoreMultiplier;
        }
        else
        {
            scoreMultiplier = 1;
        }
    }

    public void ScoreEvaluationSend(int index)
    {
        ScoreEvaluation = index;
        if (index >= 0 && index < ImageEmoji.Length)
        {
            for (int i = 0; i < ImageEmoji.Length; i++)
            {
                ImageEmoji[i].SetActive(i == index);
            }
        }
        ScoreEvaluation = index + 1;
    }

    public void SendData()
    {
        CheckFirstTime(1);
        End_Menu2.SetActive(true);
        End_Menu3.SetActive(false);
    }

    public void Cancle()
    {
        End_Menu2.SetActive(true);
        End_Menu3.SetActive(false);
    }

    public void CheckFirstTime(int index)
    {
        FirstTimePlay = index;
    }

    private IEnumerator ScaleText(TMP_Text text, float scaleFactor, float duration)
    {
        Vector3 originalScale = text.transform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            text.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = targetScale;

        // Scale back to original size
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            text.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = originalScale;
    }

    public void Setting_Button()
    {
        Setting.SetActive(true);
    }
    
    
}
