using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    
    
    [SerializeField] long price;
    public GameObject interactText;
    public GameObject interactTextFull;
    public GameObject interactTextBroke;
    public playerController controller;

    // Start is called before the first frame update
    void Start()
    {
        interactText.SetActive(false);
        interactTextFull.SetActive(false);
        interactTextBroke.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        long moneylong = gameManager.instance.GetPoints();

        bool ammoFull = true;

        foreach (var gun in controller.gunList)
        {
            if (gun.ammoMax < gun.startup && gun.ammoCurr <= gun.clip)
            {
                ammoFull = false;
                interactText.SetActive(true);
                interactTextFull.SetActive(false);
                break;
            }
        }

        if (ammoFull)
        {
            interactText.SetActive(false);

            interactTextFull.SetActive(true);

            return;
        }

        interactText.SetActive(true);

        if (Input.GetButtonDown("Interact"))
        {
            if (moneylong < price)
            {
                StartCoroutine(broke());
            }else
            {
                foreach (var gun in controller.gunList)
                {
                    gun.ammoMax = gun.startup;
                }

                controller.updatePlayerUI();

                gameManager.instance.updatePointsSub(price);

                interactText.SetActive(false);
            }
        }
           
        


        
    }

    private void OnTriggerExit(Collider other)
    {
        interactText.SetActive(false);
        interactTextFull.SetActive(false);
        interactTextBroke.SetActive(false);
    }

    IEnumerator broke()
    {
        interactTextBroke.SetActive(true);
        yield return new WaitForSeconds(.5f);
        interactTextBroke.SetActive(false);
    }
}
