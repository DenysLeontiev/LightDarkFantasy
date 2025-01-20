using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip lightFantasyAudioClip;
    [SerializeField] private AudioClip darkFantasyAudioClip;

    [Header("Configs")]
    [SerializeField] private float increaseVolumeValue = 0.01f;
    [SerializeField] private float waitTimeBetweenVolumeIncrease = 0.01f;
    [Range(0f, 1f)]
    [SerializeField] private float minVolumeValue = 0.0f;
    [Range(0f, 1f)]
    [SerializeField] private float maxVolumeValue = 1.0f;

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

    public IEnumerator PlayLightFantasy()
    {
        audioSource.Stop();
        audioSource.volume = minVolumeValue;
        audioSource.PlayOneShot(lightFantasyAudioClip);

        while (audioSource.volume < maxVolumeValue)
        {
            audioSource.volume += increaseVolumeValue;
            yield return new WaitForSeconds(waitTimeBetweenVolumeIncrease);
        }

        audioSource.volume = maxVolumeValue;
    }

    public IEnumerator PlayDarkFantasy()
    {
        audioSource.Stop();
        audioSource.volume = minVolumeValue;
        audioSource.PlayOneShot(darkFantasyAudioClip);

        while (audioSource.volume < maxVolumeValue)
        {
            audioSource.volume += increaseVolumeValue;
            yield return new WaitForSeconds(waitTimeBetweenVolumeIncrease);
        }

        audioSource.volume = maxVolumeValue;
    }
}
