using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingLevel : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject LoadingScreen;

    [Header("Slider")]
    [SerializeField] private Slider LoadingSlider;

    public void loadLevel(string level)
    {
        MainMenu.SetActive(false);

        LoadingScreen.SetActive(true);

        StartCoroutine(loadLevelASync(level));
    }

    IEnumerator loadLevelASync(string level)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(level);

        while (!loadOp.isDone)
        {
            float progress = Mathf.Clamp01(loadOp.progress / 0.9f);

            LoadingSlider.value = progress;

            yield return null;
        }
    }
}
