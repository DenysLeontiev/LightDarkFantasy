using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image faderImage;

    public const float FADE_IN_TIME = 1f;
    public const float FADE_OUT_TIME = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeOut(float time = FADE_OUT_TIME)
    {
        Color color = faderImage.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 1f, elapsedTime / time);
            faderImage.color = color;
            yield return null;
        }

        color.a = 1f;
        faderImage.color = color;
    }

    public IEnumerator FadeIn(float time = FADE_IN_TIME)
    {
        Color color = faderImage.color;
        float startAlpha = color.a;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0f, elapsedTime / time); 
            faderImage.color = color;
            yield return null;
        }

        color.a = 0f;
        faderImage.color = color;
    }
}
