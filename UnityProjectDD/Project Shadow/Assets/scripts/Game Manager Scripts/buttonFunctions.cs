using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{

    // Start is called before the first frame update
    public void resume()
    {
        gameManager.instance.stateUnPause();
    }

    // Update is called once per frame
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnPause();
    }

    public void quitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
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
}
