using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AudioClip> clips;
    private AudioSource audioSource;
    private AudioSource audioSourceBackground;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSourceBackground = GetComponentInChildren<AudioSource>();
    }

    public void PickUpBadFoodAudio()
    {
        audioSource.clip = clips[0];
        audioSource.Play();
    }
    public void PickUpGoodFoodAudio()
    {
        audioSource.clip = clips[1];
        audioSource.Play();
    }
    public void ArchBadFoodRemoveAudio()
    {
        audioSource.clip = clips[2];
        audioSource.Play();
    }
    public void ArchSellFoodAudio()
    {
        audioSource.clip = clips[3];
        audioSource.Play();
    }
    public void FinishAudio()
    {
        audioSource.clip = clips[4];
        audioSource.Play();
    }
}
