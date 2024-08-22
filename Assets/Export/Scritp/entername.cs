using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class entername : MonoBehaviour
{
    public GameObject[] Button;
    public InputField username;
    public TextMeshProUGUI output;
    public TextMeshProUGUI UserRequir;
    public GameObject feedbackText;

    // List of forbidden words
    public List<string> profaneWords = new List<string> { "badword1", "badword2", "badword3" };

    // Start is called before the first frame update
    public void Open()
    {
        Button[0].SetActive(true);
    }

    public void Off()
    {
        Button[0].SetActive(false);
        Button[1].SetActive(false);
        UserRequir.gameObject.SetActive(false);
        feedbackText.SetActive(false);
    }
    

    // This method will be called when the user tries to proceed to the next step
    public void Next()
    {
        string usernameText = username.text;

        if (string.IsNullOrEmpty(usernameText))
        {
            Debug.Log("Username is required.");
            UserRequir.gameObject.SetActive(true);
            return;
        }

        if (ContainsProfanity(usernameText))
        {
            Debug.Log("Username contains forbidden words.");
            feedbackText.SetActive(true); // Show the warning about forbidden words
            return;
        }

        output.text = usernameText;
        Button[1].SetActive(true);
        UserRequir.gameObject.SetActive(false);
        feedbackText.SetActive(false);
    }

    // Function to check if the input contains any forbidden words
    bool ContainsProfanity(string text)
    {
        foreach (string profaneWord in profaneWords)
        {
            if (text.Contains(profaneWord))
            {
                return true;
            }
        }
        return false;
    }
}
