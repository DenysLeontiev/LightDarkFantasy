using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonsHandler : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private void Start()
    {
        startGameButton.onClick.AddListener(() => StartCoroutine(LoadCharacterScene()));
    }

    private IEnumerator LoadCharacterScene()
    {
        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut();

        yield return SceneManager.LoadSceneAsync(GameScene.SelectCharacterScene.ToString());
        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();

        Destroy(gameObject);
    }
}
