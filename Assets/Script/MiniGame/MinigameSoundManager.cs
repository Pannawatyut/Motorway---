using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class MinigameSoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;  // Reference to the Audio Mixer
    public Slider bgmSlider;       // Reference to the BGM slider
    public Slider effectsSlider;   // Reference to the Effects slider

    private void Start()
    {
        // Add listeners for when the sliders are changed
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
    }

    // Function to set BGM volume
    public void SetBGMVolume(float sliderValue)
    {
        // Convert slider value (0 to 1) to decibels and set the exposed parameter in the mixer
        audioMixer.SetFloat("BGMVolume", sliderValue);
        Debug.Log("BGM Volume : " + sliderValue);
    }

    // Function to set Effects volume
    public void SetEffectsVolume(float sliderValue)
    {
        // Convert slider value (0 to 1) to decibels and set the exposed parameter in the mixer
        audioMixer.SetFloat("EffectVolume", sliderValue);
        Debug.Log("Effect Volume : " + sliderValue);
    }
}
