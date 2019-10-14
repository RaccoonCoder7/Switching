using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathSound : MonoBehaviour
{
    public BossState bs;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if(bs.deathCount == 2 && bs)
        {
            audioSource.Play();
        }
    }
}
