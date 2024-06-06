using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
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

    public bool isPaused;
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
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

    public int GetEnemyCount()
    {
        return enemyCount;
    }
}
