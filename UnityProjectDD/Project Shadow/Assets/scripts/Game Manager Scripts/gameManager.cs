using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuSound;
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject menuSkills;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text ammoCurrText;
    [SerializeField] TMP_Text points;
    [SerializeField] TMP_Text waveCountText;
    [SerializeField] TMP_Text skillPointText;
    [SerializeField] TMP_Text menuSkillPointText;
    [SerializeField] GameObject audioSourceObject;
    [SerializeField] public List<GameObject> spawntiles = new List<GameObject>();
    [SerializeField] List<GameObject> waveObjects = new List<GameObject>();
    //[SerializeField] int timeBetweenWaves;
    [SerializeField] long money;

    [SerializeField] CanvasGroup flashCanvasGroupUI;
    [SerializeField] CanvasGroup flashCanvasGroupUI1;
    [SerializeField] CanvasGroup flashCanvasGroupUI2;
    [SerializeField] CanvasGroup flashCanvasGroupUI3;
    [SerializeField] CanvasGroup flashCanvasGroupUI4;
    [SerializeField] CanvasGroup enemyCanvasGroupUI;
    [SerializeField] CanvasGroup enemyCanvasGroupNumsUI;
    [SerializeField] CanvasGroup miniMapUI;

    public GameObject playerSpawnPos;
    //public GameObject scoreScreen;


    public GameObject playerFlashDamage;
    public GameObject playerFlashDamage1;
    public GameObject playerFlashDamage2;
    public GameObject playerFlashDamage3;
    public GameObject playerFlashDamage4;
    public Image playerHPBar;
    public Image playerArmorBar;
    public Image playerStamBar;
    public GameObject checkpointPopup;
   public GameObject winFirstSelected, loseFirstSelected, pauseFirstSelected;

    public GameObject player;
    public playerController playerScript;

    public GameObject camera;

    public cameraController cameraScript;

    public int currentWave =0;
    //bool infiniteSpawnOn = true;

    public bool isPaused;

    int enemyCount;
    int waveNumber = 0;
    [SerializeField] int skillpoint;
    //int killCount = 0;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // makes gameManager consistent between scenes; commented out because it only works on root objects/components of root objects - need to adjust to make this work
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

        if (enemyCount == 0)
        {
            StartCoroutine(StartWave());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                pauseMenu();
            }
            else if (menuActive == menuPause)
            {
                stateUnPause();
            }
        }
        //Updated upstream
        if (currentWave == 5 && enemyCount == 0)
        {
            StartCoroutine(win());
        }

        if (currentWave == 5)
        {
            StopCoroutine(StartWave());
        }

        //if (Input.GetKey(KeyCode.Tab))
        //{
        //    scoreScreen.SetActive(true);
        //    UpdateScoreScreen();
        //}
        //else
        //{
        //    scoreScreen.SetActive(false);
        //}

        
    }

    //public void EnemyKilled()
    //{
    //    killCount++;
    //}

    public void NewWave()
    {
        waveNumber++;
    }

    //void UpdateScoreScreen()
    //{
    //    Text waveText = scoreScreen.transform.Find("Wave").GetComponent<Text>();
    //    Text killText = scoreScreen.transform.Find("Kill").GetComponent<Text>();

    //    waveText.text = "Wave: " + waveNumber;
    //    killText.text = "Kills: " + killCount;

    //}

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

    public void updateSkillPoints(int amount)
    {
        skillpoint += amount;
        skillPointText.text = skillpoint.ToString();
        menuSkillPointText.text = skillpoint.ToString();
    }
    public void updatePoints(long bankValue)
    {

        money += bankValue;
        points.text = money.ToString();
    }

    public void updatePointsSub(long bankValue)
    {
        money -= bankValue;
        points.text = money.ToString();
    }

    public void updateEnemyGoal(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");
    }

    public void updateWaveCountText()
    {
        waveCountText.text = currentWave.ToString("F0");
    }    

    public long GetPoints()
    {
        return money;
    }

    public int GetSkills()
    {
        return skillpoint;
    }
    IEnumerator win()
    {
        yield return new WaitForSeconds(2);
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(isPaused);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(winFirstSelected);
    }

    public void loseMenu()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(loseFirstSelected);
    }

    public void pauseMenu()
    {
        statePause();
        menuActive = menuPause;
        menuActive.SetActive(isPaused);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
    }

    public void soundMenu()
    {
        statePause();
        menuActive = menuSound;
        menuActive.SetActive(isPaused);
    }

    public void optionsMenu()
    {
        statePause();
        menuActive = menuOptions;
        menuActive.SetActive(isPaused);
    }

    public void SkillsMenu()
    {
        statePause();
        menuActive = menuSkills;
        menuActive.SetActive(isPaused);
    }

    public void ScreenFlashToggle()
    {
        if (flashCanvasGroupUI.alpha > 0)
        {
            flashCanvasGroupUI.alpha = 0;
            flashCanvasGroupUI1.alpha = 0;
            flashCanvasGroupUI2.alpha = 0;
            flashCanvasGroupUI3.alpha = 0;
            flashCanvasGroupUI4.alpha = 0;
        }
        else
        {
            flashCanvasGroupUI.alpha = 1;
            flashCanvasGroupUI1.alpha = 1;
            flashCanvasGroupUI2.alpha = 1;
            flashCanvasGroupUI3.alpha = 1;
            flashCanvasGroupUI4.alpha = 1;
        }
    }

    public void ScreenFlashResetter()
    {
        if(flashCanvasGroupUI.alpha == 0 || flashCanvasGroupUI1.alpha == 0 || flashCanvasGroupUI2.alpha == 0 || flashCanvasGroupUI3.alpha == 0 || flashCanvasGroupUI4.alpha == 0)
        {
            flashCanvasGroupUI.alpha = 1;
            flashCanvasGroupUI1.alpha = 1;
            flashCanvasGroupUI2.alpha = 1;
            flashCanvasGroupUI3.alpha = 1;
            flashCanvasGroupUI4.alpha = 1;
        }
    }
    public void EnemyCountToggle()
    {
        if (enemyCanvasGroupUI.alpha > 0)
        {
            enemyCanvasGroupUI.alpha = 0;
            enemyCanvasGroupNumsUI.alpha = 0;
        }
        else
        {
            enemyCanvasGroupUI.alpha = 1;
            enemyCanvasGroupNumsUI.alpha = 1;
        }
    }

    public void MiniToggle()
    {
        if (miniMapUI.alpha > 0)
        {
            miniMapUI.alpha = 0;
        }
        else
        {
            miniMapUI.alpha = 1;
        }
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public void RegisterSpawnTile(GameObject spawntile)
    {
        SpawnTile st = spawntile.GetComponent<SpawnTile>();
        spawntiles.Add(spawntile);


    }

    IEnumerator StartWave()
    {

        
        
        yield return new WaitForSeconds(4);

        if (enemyCount == 0)
        {
            if (waveObjects.Count > 0 && spawntiles.Count > 0)
            {
                //Debug.Log(waveObjects.Count.ToString());
                for (int i = 0; i < waveObjects.Count; i++)
                {
                    updateEnemyGoal(waveObjects.Count);
                    currentWave = i + 1;
                    updateWaveCountText();

                    updateSkillPoints(1);
                    IWave wave = waveObjects[i].GetComponent<IWave>();

                    if (wave != null)
                    {
                        //Debug.Log("got the wave");
                        wave.CreateWave();
                    }
                    else
                    {
                        //Debug.Log("couldn't get wave");
                    }



                    yield return new WaitForSeconds(10);  // wait 10 seconds for spawn
                    
                    while (enemyCount > 0)
                    { // check every 2 seconds if wave is complete and all enimies are dead.
                        yield return new WaitForSeconds(2);
                    }
                }
            }
            
        }
       
        
       // isStartWave = false;
    }

    

}
