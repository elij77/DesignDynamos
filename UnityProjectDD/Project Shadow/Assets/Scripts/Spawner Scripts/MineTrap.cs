using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrap : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform[] spawnPos;

    bool trapSet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !trapSet)
        {
            for (int i = 0; i < spawnPos.Length; i++)
            {

                Instantiate(objectToSpawn, spawnPos[i].position, spawnPos[i].rotation);

            }
            trapSet = true;
        }
    }
}