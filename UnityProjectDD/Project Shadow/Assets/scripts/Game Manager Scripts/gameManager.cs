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
    [SerializeField] GameObject menuHUD;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text ammoCurrText;
    [SerializeField] TMP_Text points;
    [SerializeField] GameObject audioSourceObject;
    [SerializeField] List<GameObject> spawntiles = new List<GameObject>();
    [SerializeField] int timeBetweenWaves;
    [SerializeField] int numberOfWaves;
    [SerializeField] int ObjectsPerWave;
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


    public GameObject playerFlashDamage;
    public GameObject playerFlashDamage1;
    public GameObject playerFlashDamage2;
    public GameObject playerFlashDamage3;
    public GameObject playerFlashDamage4;
    public Image playerHPBar;
    public Image playerArmorBar;
    public Image playerStamBar;
    public GameObject checkpointPopup;

    public GameObject player;
    public GameObject raider;
    public playerController playerScript;

    public new GameObject camera;

    public cameraController cameraScript;

    bool isStartWave;
    bool infiniteSpawnOn = true;
    public bool isPaused;
    int enemyCount;

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
        raider = GameObject.FindWithTag("Raider");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("playerSpawnPos");

        StartCoroutine(StartWave());

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

    public long GetPoints()
    {
        return money;
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

    public void pauseMenu()
    {
        statePause();
        menuActive = menuPause;
        //EventSystem.current.SetSelectedGameObject(menuPause);
        menuActive.SetActive(isPaused);
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

    public void HUDMenu()
    {
        statePause();
        menuActive = menuHUD;
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
        if(flashCanvasGroupUI.alpha > 0 || flashCanvasGroupUI1.alpha > 0 || flashCanvasGroupUI2.alpha > 0 || flashCanvasGroupUI3.alpha > 0 || flashCanvasGroupUI4.alpha > 0)
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
        isStartWave = true;
        int arrayPos;
        SpawnTile st;
        yield return new WaitForSeconds(4);

        if (numberOfWaves > 0 && ObjectsPerWave > 0 && spawntiles.Count > 0)
        {



            for (int i = 0; i < numberOfWaves; i++)
            {
                for (int j = 0; j < ObjectsPerWave; j++)
                {
                    arrayPos = Random.Range(0, spawntiles.Count);
                    st = spawntiles[arrayPos].GetComponent<SpawnTile>();
                    st.spawn();
                }


                Debug.Log("start Wave " + i.ToString());
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        else
        {
            Debug.Log("Nothing to spawn");
        }
        isStartWave = false;
    }

    // getter to determine if infinite spawncontrollers are needed.
    public bool InfiniteSpawnerOn()
    {
        return infiniteSpawnOn;
    }


}
