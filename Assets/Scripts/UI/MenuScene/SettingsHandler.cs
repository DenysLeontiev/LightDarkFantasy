using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider soundSlider;

    private void Start()
    {
        HandleBrightnessSlider();
        HandleSoundSlider();
    }

    private void HandleBrightnessSlider()
    {
        brightnessSlider.value = BrightnessManager.Instance.CurrentBrightnessValue;
        brightnessSlider.onValueChanged.AddListener(OnBrigthnessSliderValueChanged);
    }

    private void HandleSoundSlider()
    {
        soundSlider.value = MusicManager.Instance.CurrentVolumeValue;
        soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
    }

    private void OnBrigthnessSliderValueChanged(float transparencyValue)
    {
        BrightnessManager.Instance.SetImageBrightness(transparencyValue);
    }

    private void OnSoundSliderValueChanged(float soundValue)
    {
        MusicManager.Instance.SetSoundValue(soundValue);
    }
}
