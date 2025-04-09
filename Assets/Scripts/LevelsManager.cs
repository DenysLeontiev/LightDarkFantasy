using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    [System.Serializable]
    private struct Level
    {
        public string levelDisplayName;
        public Sprite levelSprite;
        public GameScene gameScene;
        public int index;
        public bool isDarkFantasy;
    }

    [SerializeField] private TMP_Dropdown levelsDropdown;
    [SerializeField] private List<Level> levels;

    private void Awake()
    {
        levelsDropdown.onValueChanged.AddListener((int levelsIndex) =>
        {
            Level level = levels.Find(l => l.index == levelsIndex);

            StartCoroutine(LoadScene(level));
        });
    }

    private void Start()
    {
        PopulateLevelsDropdown();
    }

    private void PopulateLevelsDropdown()
    {
        levelsDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> optionDatas = new();

        foreach (Level level in levels)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(level.levelDisplayName, level.levelSprite);
            optionDatas.Add(optionData);
        }

        levelsDropdown.AddOptions(optionDatas);
    }

    private IEnumerator LoadScene(Level level)
    {
        DontDestroyOnLoad(this.gameObject);

        Fader fader = FindObjectOfType<Fader>();

        bool isDarkFantasy = level.isDarkFantasy;

        if(isDarkFantasy)
        {
            StartCoroutine(MusicManager.Instance.PlayDarkFantasy());
        }
        else
        {
            StartCoroutine(MusicManager.Instance.PlayLightFantasy());
        }

        yield return fader.FadeOut();

        GetComponent<CanvasGroup>().alpha = 0f; // so current Canvas does not overlap old Canvas

        yield return SceneManager.LoadSceneAsync(level.gameScene.ToString());
        yield return new WaitForSeconds(1f);
        yield return fader.FadeIn();

        Destroy(this.gameObject);
    }
}
