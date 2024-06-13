using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;
    [SerializeField] long price;

    public GameObject interactText;
    public GameObject interactTextBroke;
    List<gunStats> tempList;
    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCurr = gun.clip;
        gun.ammoMax = gun.startup;

        interactText.SetActive(false);
        interactTextBroke.SetActive(false);
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
                gameManager.instance.updatePointsSub(price);
                

                interactText.SetActive(false);
            }
            else if (Input.GetButtonDown("Interact") && moneylong < price)
            {
                StartCoroutine(broke());
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        interactText.SetActive(false);
        interactTextBroke.SetActive(false);
    }

    IEnumerator broke()
    {
        interactTextBroke.SetActive(true);
        yield return new WaitForSeconds(.5f);
        interactTextBroke.SetActive(false);
    }
}
