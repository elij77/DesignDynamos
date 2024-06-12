using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class buttonFunctions : MonoBehaviour
{
    public TMP_Dropdown resDropDown;

    public Animator animator;

    public GameObject[] menuItems;

    Resolution[] resolutions;

    private int selectedItemIndex = 0;

    public float wait;

    // Start is called before the first frame update
    public void Start()
    {
        resolutions = Screen.resolutions;
        
        resDropDown.ClearOptions();

        List<string> resOptions = new List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string res = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(res);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height  == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resDropDown.AddOptions(resOptions);
        resDropDown.value = currentResIndex;
        resDropDown.RefreshShownValue();
    }
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedItemIndex--;

            if (selectedItemIndex < 0)
            {
                selectedItemIndex = menuItems.Length - 1;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedItemIndex++;

            if (selectedItemIndex >= menuItems.Length)
            {
                selectedItemIndex = 0;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            menuItems[selectedItemIndex].GetComponent<Button>().onClick.Invoke();
        }

        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == selectedItemIndex)
            {
                menuItems[i].GetComponent<Button>().Select();
            }
            else
            {
                menuItems[i].GetComponent<Button>().OnDeselect(null);
            }
        }
    }

   
    public void resume()
    {
        gameManager.instance.stateUnPause();
    }

    // Update is called once per frame
    public void restart()
    {
        StartCoroutine(restartLevel());
        gameManager.instance.stateUnPause();
    }

    IEnumerator restartLevel()
    {
        yield return new WaitForSeconds(0.01f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        

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

    public void quitmenu()
    {
        SceneManager.LoadScene(0);
    }

    public void respawn()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnPause();
    }

    public void Options()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.optionsMenu();
    }

    public void HUD()
    {
        gameManager.instance.HUDMenu();
    }
    public void noises()
    {
        gameManager.instance.soundMenu();
    }
    public void pauseReturn()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.pauseMenu();
    }

    public void optionsReturn()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.optionsMenu();
    }

    public void ScreenFlash()
    {
        gameManager.instance.ScreenFlashToggle();
    }

    public void CounterFlash()
    {
        gameManager.instance.EnemyCountToggle();
    }

    public void miniFlash()
    {
        gameManager.instance.MiniToggle();
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
}
