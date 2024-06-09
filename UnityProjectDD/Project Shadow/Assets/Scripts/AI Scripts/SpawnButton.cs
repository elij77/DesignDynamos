using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{

    public enemySpawner spawner;

    public float interactionDistance = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                spawner.SpawnEnemyManual();
            }
        }
    }
}
