using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialButtonsHandler : MonoBehaviour
{
    [SerializeField] private Button homeButton;
    [SerializeField] private GameScene gameSceneToLoad = GameScene.MenuScene;

    private void Start()
    {
        homeButton.onClick.AddListener(() =>
        {
            StartCoroutine(LoadScene());
        });
    }

    private IEnumerator LoadScene()
    {
        DontDestroyOnLoad(this.gameObject);

        Fader fader = FindObjectOfType<Fader>();

        yield return fader.FadeOut();
        yield return SceneManager.LoadSceneAsync(gameSceneToLoad.ToString());
        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();

        Destroy(this.gameObject);
    }
}
