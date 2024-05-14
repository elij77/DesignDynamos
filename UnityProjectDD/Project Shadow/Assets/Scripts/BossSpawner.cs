using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] GameObject bossPrefab;

    // Checks to see if boss is already in scene
    private bool bossSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.GetEnemyCount() == 0 && !bossSpawned)
        {
            Instantiate(bossPrefab, transform.position, Quaternion.identity);
            bossSpawned = true;
        }
    }
}
