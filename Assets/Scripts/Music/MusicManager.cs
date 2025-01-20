using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    [SerializeField] private AudioClip lightFantasyAudioClip;
    [SerializeField] private AudioClip darkFantasyAudioClip;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLightFantasy()
    {
        audioSource.PlayOneShot(lightFantasyAudioClip);
    }

    public void PlayDarkFantasy()
    {
        audioSource.PlayOneShot(darkFantasyAudioClip);
    }
}
