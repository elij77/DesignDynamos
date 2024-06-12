using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    public static SceneMan Instance;

    public Animator animator;
    public float wait;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            //DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel()
    {
        StartCoroutine(loadScene());
    }
    IEnumerator loadScene()
    {
        animator.SetTrigger("start");

        yield return new WaitForSeconds(wait);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
