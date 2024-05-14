using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{

    [SerializeField] int health;
     
     
    int hp;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IHeal heal = other.gameObject.GetComponent<IHeal>();
            // Increase the players health

            heal.Heal(health);
           
            Destroy(gameObject);

        }
    }

}
