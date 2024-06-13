using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int spawnTimer;
    [SerializeField] Transform spawnPos;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;


    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning  && gameManager.instance.InfiniteSpawnerOn() )
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = false;
        }
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        
        Instantiate(objectToSpawn, spawnPos.position, spawnPos.rotation);
        gameManager.instance.updateEnemyGoal(1);
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;


    }
}
