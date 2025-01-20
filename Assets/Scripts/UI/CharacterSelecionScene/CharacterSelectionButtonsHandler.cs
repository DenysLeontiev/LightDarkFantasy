using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        yield return LoadDesiredScene(GameScene.KnightTutorialScene);
    }

    private IEnumerator WaitForMageScene()
    {
        yield return StartCoroutine(MusicManager.Instance.PlayDarkFantasy());
        yield return LoadDesiredScene(GameScene.MageTutotialScene);
    }

    private IEnumerator LoadDesiredScene(GameScene desiredGameScene)
    {
        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut();

        yield return SceneManager.LoadSceneAsync(desiredGameScene.ToString());
        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();

        Destroy(gameObject);
    }
}
