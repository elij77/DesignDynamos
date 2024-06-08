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
    [SerializeField] GameObject menuSound;
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

    bool isStartWave;
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

       // StartCoroutine(StartWave());

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

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        
        if (enemyCount == 0)
        {
            StartCoroutine(win());
        }
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
        menuActive.SetActive(isPaused);
    }

    public void soundMenu()
    {
        menuActive = menuSound;
        menuActive.SetActive(isPaused);
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    //public void RegisterSpawnTile(GameObject spawntile)
    //{
    //    SpawnTile st = spawntile.GetComponent<SpawnTile>();
    //    spawntiles.Add(spawntile);


    //}

    //IEnumerator StartWave()
    //{
    //    isStartWave = false;
    //    int arrayPos;
    //    SpawnTile st;
    //    yield return new WaitForSeconds(4);

    //    for (int i = 0; i < numberOfWaves; i++)
    //    {
    //        for (int j = 0; j < ObjectsPerWave; j++)
    //        {
    //            arrayPos = Random.Range(0, spawntiles.Count);
    //            st = spawntiles[arrayPos].GetComponent<SpawnTile>();
    //            st.spawn();
    //        }


    //        Debug.Log("start Wave " + i.ToString());
    //        yield return new WaitForSeconds(timeBetweenWaves);
    //    }

    //    isStartWave = false;
    //}



}
