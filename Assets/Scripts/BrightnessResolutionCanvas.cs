using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessResolutionCanvas : MonoBehaviour
{
    public static BrightnessResolutionCanvas Instance { get; private set; }

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
}
