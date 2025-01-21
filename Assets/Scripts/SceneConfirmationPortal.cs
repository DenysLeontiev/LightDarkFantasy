using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneConfirmationPortal : MonoBehaviour
{
    [SerializeField] private GameObject confirmationWindow;
    [SerializeField] private GameScene sceneToLoad;
    private Transform confirmationPanel;

    private void Start()
    {
        HideConfirmationWindow();
        confirmationPanel = confirmationWindow.transform.Find("Panel");

        confirmationPanel.Find("YesButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            StartCoroutine(AgreeToGoNextLevel());
        });

        confirmationPanel.Find("NoButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            DisagreeToGoNextLevel();
        });


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerBase>(out PlayerBase playerBase))
        {
            ShowConfirmationWindow();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerBase>(out PlayerBase playerBase))
        {
            HideConfirmationWindow();
        }
    }

    private IEnumerator AgreeToGoNextLevel()
    {
        DontDestroyOnLoad(this.gameObject);

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut();
        yield return SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();

        Destroy(this.gameObject);
    }

    private void DisagreeToGoNextLevel()
    {
        HideConfirmationWindow();
    }

    private void ShowConfirmationWindow()
    {
        if(confirmationWindow != null)
            confirmationWindow.SetActive(true);
    }

    private void HideConfirmationWindow()
    {
        if(confirmationWindow != null)
            confirmationWindow.SetActive(false);
    }
}
