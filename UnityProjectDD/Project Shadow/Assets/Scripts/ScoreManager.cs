using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text killsText;
    public Text waveNumberText;

    int kills = 0;
    int waveNumber = 0;

    bool isVisible = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        killsText.text = kills.ToString() + " KILLS";
        waveNumberText.text = "WAVE: " + waveNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isVisible = !isVisible;
            killsText.gameObject.SetActive(isVisible);
        }
    }

    public void AddKill()
    {
        kills += 1;
        killsText.text = kills.ToString() + " KILLS";
    }


    //public void UpdateWaveNumber(int waveNumber)
    //{
    //    this.waveNumber = waveNumber;
    //    waveNumberText.text = "Wave: " + waveNumber.ToString();
    //}
}
