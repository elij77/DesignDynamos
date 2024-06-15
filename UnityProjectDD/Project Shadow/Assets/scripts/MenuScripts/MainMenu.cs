using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Dropdown resDropDown;

    public Animator animator;

    Resolution[] resolutions;

    public GameObject menuFirst, playFirst, settingsFirst, volumeFirst,
                      graphicsFirst, mainMenu, PlayMenu, SettingsMenu,
                      VolumeMenu, GraphicsMenu;

    public float wait;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;

        resDropDown.ClearOptions();

        List<string> resOptions = new List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string res = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(res);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resDropDown.AddOptions(resOptions);
        resDropDown.value = currentResIndex;
        resDropDown.RefreshShownValue();

        animator.enabled = true;

        AudioManager.Instance.playMusic("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGame()
    {
        StartCoroutine(loadScene());
        AudioManager.Instance.animator.SetTrigger("FadeIn");

    }

    IEnumerator loadScene()
    {
        animator.SetTrigger("start");

        yield return new WaitForSeconds(wait);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void setGraphics(int Quality)
    {
        QualitySettings.SetQualityLevel(Quality);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void MenuMain()
    {
        if (mainMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(menuFirst);
        }
    }

    public void playMenu()
    {
        if (PlayMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(playFirst);
        }
    }

    public void settingsMenu() 
    {
        if (SettingsMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(settingsFirst);
        }
    }

    public void volumeMenu()
    {
        if (VolumeMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(volumeFirst);
        }
    }

    public void graphicsMenu()
    {
        if (GraphicsMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(graphicsFirst);
        }
    }
}
