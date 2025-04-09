using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance { get; private set; }

    public float CurrentBrightnessValue { get; private set ; }

    [SerializeField] private Image brightnessImage;

    [SerializeField] private float initialBrightnessValue = 1f;
    [SerializeField] private float maxBrightnessValue = 0.9f;
    [SerializeField] private float minBrightnessValue = 0f;

    private void Awake()
    {
        CurrentBrightnessValue = initialBrightnessValue;
        SetImageBrightness(initialBrightnessValue);

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

    public void SetImageBrightness(float a)
    {
        CurrentBrightnessValue = a;
        a = Mathf.Lerp(maxBrightnessValue, minBrightnessValue, a);
        a = Mathf.Clamp(a, minBrightnessValue, maxBrightnessValue);
        Color imageColor = brightnessImage.color;
        imageColor.a = a;
        brightnessImage.color = imageColor;
    }
}
