using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator animator;

    public GameObject[] menuItems;

    private int selectedItemIndex = 0;

    public float wait;
    public void Start()
    {
        
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
    public void playGame ()
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

    public void QuitGame()
    {
        
    }
}
