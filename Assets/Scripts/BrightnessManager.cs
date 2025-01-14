using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance { get; private set; }

    [SerializeField] private Image brightnessImage;
    [SerializeField] private Slider brightnessSlider;

    [SerializeField] private float initialBrightnessValue = 1f;
    [SerializeField] private float maxBrightnessValue = 0.9f;
    [SerializeField] private float minBrightnessValue = 0f;

    private string sliderValueKey = "sliderValueKey";

    private void Awake()
    {
        if (Instance != null && Instance != this) // Destroy another object
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        brightnessSlider.onValueChanged.AddListener(OnSliderValueChanged);
        SetImageBrightness(initialBrightnessValue);
        brightnessSlider.value = initialBrightnessValue;
    }

    private void OnSliderValueChanged(float transparencyValue)
    {
        SetImageBrightness(transparencyValue);
    }

    private void SetImageBrightness(float a)
    {
        a = Mathf.Lerp(maxBrightnessValue, minBrightnessValue, a);
        a = Mathf.Clamp(a, minBrightnessValue, maxBrightnessValue);
        Color imageColor = brightnessImage.color;
        imageColor.a = a;
        brightnessImage.color = imageColor;
    }
}
