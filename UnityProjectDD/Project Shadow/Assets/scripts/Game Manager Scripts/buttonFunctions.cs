using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;


public class buttonFunctions : MonoBehaviour
{
    public Animator animator;

    public GameObject optionsFirstSelected, HUDFirstSelected, soundFirstSelected, optionsClosedSelected, HUDClosedSelected, soundClosedSelected;

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

    public void quitmenu()
    {
        AudioManager.Instance.animator.SetTrigger("FadeIn");
        StartCoroutine(quitToMain());
        //gameManager.instance.stateUnPause();

    }

    public void winBackMain()
    {
        SceneManager.LoadScene(0);
        
    }

    IEnumerator quitToMain()
    {
        gameManager.instance.stateUnPause();
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);

    }

    public void HUD()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.HUDMenu();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(HUDFirstSelected);
    }
    public void noises()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.soundMenu();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(soundFirstSelected);
    }
    public void pauseReturn()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.pauseMenu();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsClosedSelected);
    }

    public void optionsReturnFromHUD()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.optionsMenu();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(HUDClosedSelected);
    }
    public void optionsReturnFromSound()
    {
        gameManager.instance.stateUnPause();
        gameManager.instance.optionsMenu();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(soundClosedSelected);
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
}
