using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public const float TIME_TO_MAX_VOLUME = 1f;

    private AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip lightFantasyAudioClip;
    [SerializeField] private AudioClip darkFantasyAudioClip;

    [Header("Configs")]
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
        audioSource.loop = true;
    }

    public IEnumerator PlayLightFantasy(float durationInSeconds = TIME_TO_MAX_VOLUME)
    {
        if(audioSource.clip == lightFantasyAudioClip)
            yield break;

        audioSource.volume = minVolumeValue;
        audioSource.clip = lightFantasyAudioClip;
        audioSource.loop = true;
        audioSource.Play();

        yield return AdjustVolumeOverTime(maxVolumeValue, durationInSeconds);
    }

    public IEnumerator PlayDarkFantasy(float durationInSeconds = TIME_TO_MAX_VOLUME)
    {
        if(audioSource.clip == darkFantasyAudioClip)
            yield break;

        audioSource.volume = minVolumeValue;
        audioSource.clip = darkFantasyAudioClip;
        audioSource.loop = true;
        audioSource.Play();

        yield return AdjustVolumeOverTime(maxVolumeValue, durationInSeconds);
    }

    private IEnumerator AdjustVolumeOverTime(float targetVolume, float duration)
    {
        float initialVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(initialVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
