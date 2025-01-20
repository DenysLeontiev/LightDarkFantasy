using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameScene sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerBase playerBase = collision.GetComponent<PlayerBase>();

        if(playerBase != null)
        {
            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene()
    {
        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut();

        yield return SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();

        Destroy(gameObject);
    }
}
