using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelOptionUI : IPointerDownHandler
{
    private GameScene gameScene;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameScene);
    }
}
