using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LaserVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    private AudioSource audioSouce;
    private AudioMixerGroup[] audioMixGroup;
    private GameObject[] laserStart;

    void Start()
    {
        laserStart = GameObject.FindGameObjectsWithTag("LASER");
        if (laserStart.Length >= 4)
        {
            audioSouce = GetComponent<AudioSource>();
            audioMixGroup = audioMixer.FindMatchingGroups("Master");
            audioSouce.outputAudioMixerGroup = audioMixGroup[0];
        }
    }
}
