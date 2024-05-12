using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject enemyCountText;


    public GameObject playerFlashDamage;
    public Image playerHPBar;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void statePause()
    {

    }

    public void stateUnPause()
    {

    }

    public void updateGameGoal()
    {

    }

    public void loseMenu()
    {
        
    }
}
