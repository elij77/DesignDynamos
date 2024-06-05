using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickUp : MonoBehaviour
{
    [SerializeField] int armor;

    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.playerArmorBar.fillAmount != 1)
        {
            IDefense shield = other.gameObject.GetComponent<IDefense>();

            shield.RepairArmor(armor);

            Destroy(gameObject);
        }
    }

}
