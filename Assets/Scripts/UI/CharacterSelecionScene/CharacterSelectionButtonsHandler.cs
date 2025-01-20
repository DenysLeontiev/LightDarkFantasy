using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionButtonsHandler : MonoBehaviour
{
    [SerializeField] private Button knightButton;
    [SerializeField] private Button mageButton;

    private void Start()
    {
        knightButton.onClick.AddListener(() =>
        {
            StartCoroutine(WaitForKnightScene());
        });

        mageButton.onClick.AddListener(() =>
        {
            StartCoroutine(WaitForMageScene());
        });
    }

    private IEnumerator WaitForKnightScene()
    {
        yield return StartCoroutine(MusicManager.Instance.PlayLightFantasy());
        SceneManager.LoadScene(GameScene.KnightTutorialScene.ToString());
    }

    private IEnumerator WaitForMageScene()
    {
        yield return StartCoroutine(MusicManager.Instance.PlayLightFantasy());
        SceneManager.LoadScene(GameScene.MageTutotialScene.ToString());
    }
}
