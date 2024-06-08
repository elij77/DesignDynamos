using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.RegisterSpawnTile(gameObject);

        Debug.Log("registertile");
    }

   

    public void spawn()
    {
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        Instantiate(objectToSpawn, currentPosition, currentRotation);
    }
}
