using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
   public static VolumeSetting Instance;
   
   public Sound[] musicSound, sfxSounds;
   public AudioSource musicSource , sfxSource;

   private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Start()
   {
      PlayeMusic("Theme");
   }

   public void PlayeMusic(string name)
   {
      Sound s = Array.Find(musicSound, x => x.name == name);
      if (s == null)
      {
         Debug.Log("Sound Not Found");
      }
      else
      {
         musicSource.clip = s.clip;
         musicSource.Play();
      }
   }

   public void PlaySFX(string name)
   {
      Sound s = Array.Find(sfxSounds, x => x.name == name);
      if (s == null)
      {
         Debug.Log("Sound Not Found");
      }
      else
      {
         sfxSource.PlayOneShot(s.clip);
      }
   }

   public void ToggleMusic()
   {
      musicSource.mute = !musicSource.mute;
   }

   public void ToggleSFX()
   {
      sfxSource.mute = !sfxSource.mute;
   }

   public void MusicVolume(float volume)
   {
      musicSource.volume = volume;
   }

   public void SFXVolume(float volume)
   {
      sfxSource.volume = volume;
   }
}
