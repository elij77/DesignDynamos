using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;

    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCurr = gun.ammoMax;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.getGunStats(gun);
            Destroy(gameObject);
        }
    }
}
