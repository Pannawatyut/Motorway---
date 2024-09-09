using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private Sound[] soundEffects;
    [SerializeField] private Sound[] backgroundMusic;

    [SerializeField] private Slider soundEffectsSlider; // Slider for sound effects
    [SerializeField] private Slider backgroundMusicSlider; // Slider for background music

    [Serializable] public struct Sound
    {
        public SoundName soundName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume;
        public bool loop;
        [HideInInspector] public AudioSource audioSource;
    }

    private AudioSource currentMusicSource;
    private SoundName currentNPCSound; // Store current NPC sound

    private float soundEffectsVolume = 1f;
    private float backgroundMusicVolume = 1f;

    public enum SoundName
    {
        // Sound Effects
        NPC1,
        NPC2,
        NPC3,
        NPC4,
        NPC5,
        NPC6,
        NPC7,
        NPC8,
        NPC9,
        
        // Background Music
        BGM1,
        BGM2,

        // Minigame Sound
        Correct,
        Incorrect,
        SmallCar,
        MediumCar,
        LargeCar
    }

    public void PlaySoundEffect(SoundName soundName)
    {
        Sound sound = GetSound(soundName, soundEffects);
        if (sound.audioSource == null)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
        }
        sound.audioSource.clip = sound.clip;
        sound.audioSource.volume = sound.volume * soundEffectsVolume; // Adjust volume with current setting
        sound.audioSource.loop = false; // Sound effects typically do not loop
        sound.audioSource.Play();
    }

    public void PlayBackgroundMusic(SoundName soundName)
    {
        if (currentMusicSource != null) // Stop previous music
        {
            Destroy(currentMusicSource);
        }

        Sound sound = GetSound(soundName, backgroundMusic);
        currentMusicSource = gameObject.AddComponent<AudioSource>();
        currentMusicSource.clip = sound.clip;
        currentMusicSource.volume = sound.volume * backgroundMusicVolume; // Adjust volume with current setting
        currentMusicSource.loop = true; // Background music typically loops
        currentMusicSource.Play();
    }

    private Sound GetSound(SoundName soundName, Sound[] soundArray)
    {
        return Array.Find(soundArray, s => s.soundName == soundName);
    }

    void Start()
    {
        if (backgroundMusicSlider != null)
        {
            backgroundMusicSlider.onValueChanged.AddListener(SetBackgroundMusicVolume);
        }
        if (soundEffectsSlider != null)
        {
            soundEffectsSlider.onValueChanged.AddListener(SetSoundEffectsVolume);
        }

        // Initialize volume settings from sliders
        if (backgroundMusicSlider != null)
        {
            backgroundMusicVolume = backgroundMusicSlider.value;
        }
        if (soundEffectsSlider != null)
        {
            soundEffectsVolume = soundEffectsSlider.value;
        }

        ApplyVolumeSettings(); // Apply initial volume settings
        PlayBackgroundMusic(SoundName.BGM1);
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicVolume = volume;
        if (currentMusicSource != null)
        {
            currentMusicSource.volume = backgroundMusicVolume;
        }
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume;
        foreach (var sound in soundEffects)
        {
            if (sound.audioSource != null)
            {
                sound.audioSource.volume = sound.volume * soundEffectsVolume;
            }
        }
    }

    public void ApplyVolumeSettings()
    {
        SetSoundEffectsVolume(soundEffectsVolume); // Apply to existing sound effects
        SetBackgroundMusicVolume(backgroundMusicVolume); // Apply to existing background music
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the SoundManager persists across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    // Method to set current NPC sound
    public void SetCurrentNPCSound(SoundName soundName)
    {
        currentNPCSound = soundName;
    }

    // Method to get current NPC sound
    public SoundName GetCurrentNPCSound()
    {
        return currentNPCSound;
    }

    public bool GetCurrentSoundEffectPlaying()
    {
        foreach (var sound in soundEffects)
        {
            if (sound.audioSource != null && sound.audioSource.isPlaying)
            {
                return true;
            }
        }
        return false;
    }
}
