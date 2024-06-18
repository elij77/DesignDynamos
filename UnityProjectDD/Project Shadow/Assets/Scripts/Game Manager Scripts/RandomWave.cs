using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWave : MonoBehaviour, IWave
{
    [SerializeField] public int ObjectsPerWave;
    SpawnTile st;
    int arrayPos;

    public void CreateWave()
    {
        Debug.Log("waveobject");
        for (int j = 0; j < ObjectsPerWave; j++)
        {
            arrayPos = Random.Range(0, gameManager.instance.spawntiles.Count);
            st = gameManager.instance.spawntiles[arrayPos].GetComponent<SpawnTile>();
            if (st != null)
            {
                st.spawn();
            }
        }

    }
}
