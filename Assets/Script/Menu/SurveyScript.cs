using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SurveyScript : MonoBehaviour
{
    public Toggle[] toggles;
    public int Score;

    void Start()
    {
        // Add listeners to all toggles
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i; // Capture the current index
            toggles[i].onValueChanged.AddListener((isOn) => OnToggleValueChanged(index, isOn));
        }
    }

    void OnToggleValueChanged(int toggleIndex, bool isOn)
    {
        if (isOn)
        {
            // Update the score based on the selected toggle
            Score = toggleIndex + 1;

            // Deactivate all other toggles
            for (int i = 0; i < toggles.Length; i++)
            {
                if (i != toggleIndex)
                {
                    toggles[i].isOn = false;
                }
            }

            Debug.Log("Score updated: " + Score);
        }
    }
}
