using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] charecter;
    public int selectedCharter = 0;
    public GameObject[] button;
    public GameObject[] SkinColor;

    public void Men()
    {
        charecter[selectedCharter].SetActive(false);
        selectedCharter = (selectedCharter + 1) % charecter.Length;
        charecter[selectedCharter].SetActive(true);
        button[0].SetActive(true);
        button[1].SetActive(false);
        SkinColor[0].SetActive(true);
        SkinColor[1].SetActive(false);
    }

    public void Women()
    {
        charecter[selectedCharter].SetActive(false);
        button[0].SetActive(false);
        SkinColor[0].SetActive(false);
        selectedCharter--;
        if (selectedCharter < 0)
        {
            selectedCharter += charecter.Length;
        }
        charecter[selectedCharter].SetActive(true);
        button[1].SetActive(true);
        SkinColor[1].SetActive(true);
    }

    public void Next()
    {
        PlayerPrefs.SetInt("selectedCharter", selectedCharter);

        // Load specific scene based on selectedCharter
        if (selectedCharter == 0)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else if (selectedCharter == 1)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("No scene configured for selected character.");
        }
    }
}
