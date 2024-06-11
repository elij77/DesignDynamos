using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;

    public float wait;
    public void Start()
    {
        AudioManager.Instance.playMusic("Main Menu");
    }
    public void playGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator loadScene()
    {
        animator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(wait);

        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
