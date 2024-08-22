using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sliderbar : MonoBehaviour
{
    public Slider slider; // Reference to the UI Slider
    public TextMeshProUGUI valueText; // Reference to the UI Text

    void Start()
    {
        // Ensure the text is initialized with the current slider value
        UpdateSliderValueText(slider.value);

        // Add a listener to update the text when the slider value changes
        slider.onValueChanged.AddListener(UpdateSliderValueText);
    }

    void UpdateSliderValueText(float value)
    {
        // Update the text component with the slider's current value
        valueText.text = "Load " + (value * 100).ToString("F0") + "%"; // "F0" formats the value as an integer
    }

    void OnDestroy()
    {
        // Remove the listener when the object is destroyed to avoid memory leaks
        slider.onValueChanged.RemoveListener(UpdateSliderValueText);
    }
}
