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
        EventSystem.current.SetSelectedGameObject(null);

        
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
}
