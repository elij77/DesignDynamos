using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public GameObject player;

    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        player = GameObject.FindWithTag("Player");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
