using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuOptions;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text ammoCurrText;
    [SerializeField] GameObject audioSourceObject;

    public GameObject playerSpawnPos;
    public GameObject playerFlashDamage;
    public Image playerHPBar;
    public Image playerStamBar;
    public Image playerArmorBar;
    public GameObject checkpointPopup;

    public GameObject player;
    public playerController playerScript;

    public GameObject camera;
    public cameraController cameraScript;

    public bool isPaused;
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this); // makes gameManager consistent between scenes; commented out because it only works on root objects/components of root objects - need to adjust to make this work
        } 
        else if (instance != this)
        {
            Destroy(this);
        }

        camera = GameObject.FindWithTag("MainCamera");
        cameraScript = camera.GetComponent<cameraController>();

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("playerSpawnPos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnPause();
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }

    public void updateAmmo(int curAmount, int clip)
    {
        ammoCurrText.text = curAmount.ToString();
        ammoText.text = clip.ToString();
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        
        if (enemyCount == 0)
        {
            StartCoroutine(win());
        }
    }

    IEnumerator win()
    {
        yield return new WaitForSeconds(2);
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(isPaused);
    }

    public void loseMenu()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
        
    }

    public void optionsMenu()
    {
        stateUnPause();
        statePause();
        menuActive = menuOptions;
        menuActive.SetActive(isPaused);
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }
}
