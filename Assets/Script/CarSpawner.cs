using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CarSpawner : MonoBehaviour
{
    public float time = 120f;
    public GameObject[] FaceNPC;
    public GameObject[] carPrefabs;
    public Vector3 spawnPosition = new Vector3(0, 0, 400);
    public Vector3 targetStartPosition = new Vector3(0, 0, 16);
    public float spacing = 5f;
    public float moveDuration = 300f;

    private bool isGameOverChecked = false;

    public TextMeshProUGUI CarCount;

    public GameObject[] CheckCurrent1;

    public float indexCars = 0;
    public float car1 = 20;

    public GameObject Barrier;

    public Animator animatorBarrier;
    public Animator animatorNPC;

    public List<GameObject> spawnedCars = new List<GameObject>();
    private float elapsedTime = 0f;
    private float spawnInterval = 1f;
    private bool isCheckingCar = false;
    private bool isStart = false;
    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Slider slider1;
    public UnityEngine.UI.Image sliderFill;

    public bool _isStart;
    public AudioSource _ButtonSound;

    private float answerStartTime;

    public MinigameAudioScript _AudioScript;
    private void Start()
    {
        //cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void _StartGame()
    {
        _isStart = true;
        if (!isStart)
        {
            answerStartTime = Time.time;  // Start the timer
            isStart = true;        // Set the flag to indicate the timer is running
            Debug.Log("Timer started.");
        }
        GetComponent<ScoreManager>()._CallStarter();
        StartCoroutine(SpawnCarsContinuously());


    }

    private void Update()
    {
        if (_AudioScript == null)
        {
            _AudioScript = FindAnyObjectByType<MinigameAudioScript>();
        }
        if (_isStart)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 90f)
            {
                spawnInterval = 0.5f;
            }
            else if (elapsedTime >= 60f)
            {
                spawnInterval = 0.8f;
            }

            UpdateUI();

            if (spawnedCars.Count >= 21 && !isGameOverChecked)
            {
                GameOver();
            }
            
        }
        
    }

    private void UpdateUI()
    {
        if (spawnedCars.Count < 10)
        {
            FaceNPC[0].SetActive(true);
            FaceNPC[1].SetActive(false);
        }
        else if (spawnedCars.Count >= 10 && spawnedCars.Count < 21)
        {
            FaceNPC[0].SetActive(false);
            FaceNPC[1].SetActive(true);
        }

        CarCount.text = $"{spawnedCars.Count}/20";

        if (spawnedCars.Count >= 21)
        {
            FaceNPC[0].SetActive(false);
            FaceNPC[1].SetActive(false);
            FaceNPC[2].SetActive(true);
            CarCount.text = "GAME OVER";
        }

        indexCars = spawnedCars.Count;
        slider1.value = car1;
        slider.value = indexCars;

        float t = slider.value / slider.maxValue;
        sliderFill.color = Color.Lerp(Color.green, Color.red, t);
    }

    private IEnumerator SpawnCarsContinuously()
    {
        while (spawnedCars.Count < 21)
        {
            car1--;
            SpawnCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public GameObject _Effect;
    public void SpawnCar()
    {
        Vector3 position = spawnPosition;
        int randomIndex = Random.Range(0, carPrefabs.Length);
        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        GameObject spawnedCar = Instantiate(carPrefabs[randomIndex], position, rotation);
        
        spawnedCars.Add(spawnedCar);

        Vector3 targetPosition = targetStartPosition + new Vector3(0, 0, spawnedCars.Count * spacing);
        StartCoroutine(MoveCarToPosition(spawnedCar, targetPosition));
    }

    private IEnumerator MoveCarToPosition(GameObject car, Vector3 targetPosition)
    {
        Vector3 initialPosition = car.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            car.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        car.transform.position = targetPosition;
    }

    public void OnClickPlayAgain()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void OnClickTo_Lobby()
    {
        // Add lobby navigation logic
    }

    public void StartChecking()
    {
        answerStartTime = Time.time; // Record the start time when question is presented
    }

    public Transform _EffectPos;
    public void CheckAndDestroyFirstCar(int buttonValue)
    {
        if (isCheckingCar) return;

        isCheckingCar = true;

        if (spawnedCars.Count == 0)
        {
            Debug.LogWarning("No cars to check and destroy.");
            isCheckingCar = false;
            return;
        }

        int prefabIndex = GetCarPrefabIndex(spawnedCars[0].name);

        if (buttonValue == prefabIndex)
        {
            SetCheckCurrent1Active(true, false, true, false);

            // Calculate the time taken to answer
            float timeTaken = Time.time - answerStartTime;
            int score = CalculateScore(timeTaken);  // Calculate score based on time
            ScoreManager.Instance.AddScore(score);  // Add calculated score
            ScoreManager.Instance.CorrectAnswer();

            GameObject Effect = Instantiate(_Effect, _EffectPos.transform.position, Quaternion.identity);
            Destroy(Effect, 3f);

            animatorBarrier.Play("GateOpen");
            animatorNPC.Play("female_nod_stand");
            _AudioScript._CorrectSound.Play();
        }
        else
        {
            SetCheckCurrent1Active(true, true, false, true);
            GetComponent<ScoreManager>().time -= 1f;
            ScoreManager.Instance.WrongAnswer();
            animatorBarrier.Play("GateOpen");
            animatorNPC.Play("female_say_no");
            _AudioScript._IncorrectSound.Play();
        }

        PlayCarSound();
        StartCoroutine(MoveAndDestroyFirstCar(spawnedCars[0]));
    }
    private void PlayCarSound()
    {
        int carIndex = spawnedCars[0].GetComponentInChildren<playanimation>().CarIndex;
        switch (carIndex)
        {
            case 0:
                _AudioScript._SmallCarSound.Play();
                break;
            case 1:
                _AudioScript._MediumCarSound.Play();
                break;
            case 2:
                _AudioScript._LargeCarSound.Play();
                break;
        }
    }

    private int CalculateScore(float timeTaken)
    {
        if (timeTaken <= 1f)
        {
            return 110;
        }
        else if (timeTaken <= 2f)
        {
            return 100;
        }
        else if (timeTaken <= 3f)
        {
            return 90;
        }
        else if (timeTaken <= 4f)
        {
            return 80;
        }
        else
        {
            return 70;
        }
    }

    private int GetCarPrefabIndex(string carName)
    {
        for (int i = 0; i < carPrefabs.Length; i++)
        {
            if (carName.Contains(carPrefabs[i].name))
            {
                return i;
            }
        }
        return -1;
    }

    private void SetCheckCurrent1Active(bool a, bool b, bool c, bool d)
    {
        CheckCurrent1[0].SetActive(a);
        CheckCurrent1[1].SetActive(b);
        CheckCurrent1[2].SetActive(c);
        CheckCurrent1[3].SetActive(d);
    }

    private IEnumerator MoveAndDestroyFirstCar(GameObject car)
    {
        yield return new WaitForSeconds(0f);
        SetCheckCurrent1Active(false, false, false, false);
        Vector3 initialPosition = car.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, 0, -20);

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            car.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        car.transform.position = targetPosition;
        Destroy(car);
        spawnedCars.Remove(car);
        animatorBarrier.Play("GateOff");

        StartCoroutine(MoveCarsForward());
        yield return new WaitForSeconds(0f);
        answerStartTime = Time.time;

        yield return new WaitForSeconds(0f);
        isCheckingCar = false;
    }

    private IEnumerator MoveCarsForward()
    {
        List<Vector3> initialPositions = new List<Vector3>();
        List<Vector3> targetPositions = new List<Vector3>();

        for (int i = 0; i < spawnedCars.Count; i++)
        {
            initialPositions.Add(spawnedCars[i].transform.position);
            Vector3 newPosition = targetStartPosition + new Vector3(0, 0, i * spacing);
            targetPositions.Add(newPosition);
        }

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            for (int i = 0; i < spawnedCars.Count; i++)
            {
                if (i < initialPositions.Count && i < targetPositions.Count)
                {
                    spawnedCars[i].transform.position = Vector3.Lerp(initialPositions[i], targetPositions[i], elapsedTime / moveDuration);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < spawnedCars.Count; i++)
        {
            if (i < targetPositions.Count)
            {
                spawnedCars[i].transform.position = targetPositions[i];
            }
        }
        animatorNPC.Play("female_nod_stand");
    }

    private void GameOver()
    {
        if (isGameOverChecked) return;

        isGameOverChecked = true;
        time = 0;
        ScoreManager.Instance.OnTimerEnd();
        _isStart = false;
        Debug.Log("Game Over! Maximum number of cars reached.");
    }

    public void _BackToMain()
    {
        Application.LoadLevel("Game");
    }
}