using UnityEngine;
using System;
public class AudioManager1 : MonoBehaviour
{
    private AudioSource[] allAudioSources;

    void Start()
    {
        // หา AudioSource ทั้งหมดใน Scene
        allAudioSources = FindObjectsOfType<AudioSource>();

        // ปิดเสียงทั้งหมด
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
    }
}