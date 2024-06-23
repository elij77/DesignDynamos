using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.ProBuilder;

public class LoadingLevel : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private GameObject WinMenu;
    [SerializeField] private GameObject LoseMenu;

    

    [Header("Slider")]
    [SerializeField] private Slider LoadingSlider;

    bool isPaused;

    public void loadLevel(string level)
    {
        MainMenu.SetActive(false);

        LoadingScreen.SetActive(true);

        StartCoroutine(loadLevelASync(level));

        UnPause();
    }

    public void loadMain(string level)
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

            yield return new WaitForSeconds(2);
        }
    }

    public void loadWinLevel(string level)
    {
        WinMenu.SetActive(false);

        LoadingScreen.SetActive(true);

        StartCoroutine(loadLevelASync(level));
    }

    public void loadLoseLevel(string level)
    {
        LoseMenu.SetActive(false);

        LoadingScreen.SetActive(true);

        StartCoroutine(loadLevelASync(level));
    }

    public void UnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        


    }
}
