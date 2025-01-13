using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialButtonsHandler : MonoBehaviour
{
    [SerializeField] private Button homeButton;

    private void Start()
    {
        homeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(GameScene.MenuScene.ToString());
        });
    }
}
