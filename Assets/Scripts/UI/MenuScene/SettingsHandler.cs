using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;

    private void Start()
    {
        brightnessSlider.value = BrightnessManager.Instance.BrightnessValue;
        brightnessSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float transparencyValue)
    {
        BrightnessManager.Instance.SetImageBrightness(transparencyValue);
    }
}
