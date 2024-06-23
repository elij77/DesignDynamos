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

        interactText.SetActive(true);
        for (int j = 0; j < controller.gunList.Count; j++)
        {
            if (Input.GetButtonDown("Interact") && moneylong < price)
            {
                StartCoroutine(broke());
            }
            else if (Input.GetButtonDown("Interact") && moneylong >= price)
            {

                for (int i = 0; i < controller.gunList.Count; i++)
                {
                    controller.gunList[i].ammoMax = controller.gunList[i].startup;
                }

                //if (other.gameObject.tag == "Ammo" && controller.gunList.Capacity > 0 && controller.gunList[j].ammoMax < controller.gunList[j].startup)
                //{
                //  for (int i = 0; i < controller.gunList.Count; i++)
                //  {
                //   controller.gunList[i].ammoMax = controller.gunList[i].startup;
                //  }

                //}

                controller.updatePlayerUI();
                
             gameManager.instance.updatePointsSub(price);


             interactText.SetActive(false);
            }
            else if (controller.gunList[j].ammoMax == controller.gunList[j].startup)
            {
                interactText.SetActive(false);
                interactTextFull.SetActive(true);
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
