using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerManager : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;

    public void ToggleMusic()
    {
        VolumeSetting.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        VolumeSetting.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        VolumeSetting.Instance.MusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        VolumeSetting.Instance.SFXVolume(sfxSlider.value);
    }
}
