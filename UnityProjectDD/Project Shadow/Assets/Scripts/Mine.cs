using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    [SerializeField] int damage;
    
    int hp;
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boom");
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            IDamage dmg = other.gameObject.GetComponent<IDamage>();
            // decreases the players health

            dmg.takeDamage(damage);
            Destroy(gameObject);

        }
    }

}
