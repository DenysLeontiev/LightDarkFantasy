using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SreenResolutionDropDownOption : MonoBehaviour, IPointerDownHandler
{
    private int screenResolutionIndex;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Index " + screenResolutionIndex);
    }

    public void SetScreenResolutionIndex(int index)
    {
        screenResolutionIndex = index;
    }
}
