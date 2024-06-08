using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;
    [SerializeField] long price;

    public GameObject interactText;
    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCurr = gun.clip;
        gun.ammoMax = gun.startup;

        interactText.SetActive(false);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.SetActive(true);
            long moneylong = gameManager.instance.GetPoints();
            if (Input.GetButtonDown("Interact") && moneylong >= price)
            {
                gameManager.instance.playerScript.getGunStats(gun);
                Destroy(gameObject);
                gameManager.instance.updatePoints(-moneylong);
                

                interactText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        interactText.SetActive(false);
    }
}
