using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArmorPickUp : MonoBehaviour
{
    [SerializeField] int armor;
    [SerializeField] long price;
    public GameObject interactText;
    public GameObject interactTextFull;
    public GameObject interactTextBroke;


    private void Start()
    {
        interactText.SetActive(false);
        interactTextFull.SetActive(false);
        interactTextBroke.SetActive(false);
    }
    public void OnTriggerStay(Collider other)
    {
        long moneylong = gameManager.instance.GetPoints();

        if (other.CompareTag("Player"))
        {

            if (gameManager.instance.playerArmorBar.fillAmount != 1)
            {
                interactText.SetActive(true);
                if (Input.GetButtonDown("Interact") && moneylong < price)
                {
                    StartCoroutine(broke());
                }
                else if (Input.GetButtonDown("Interact") && moneylong >= price)
                {
                    IDefense shield = other.gameObject.GetComponent<IDefense>();

                    shield.RepairArmor(armor);

                    Destroy(gameObject);

                    gameManager.instance.updatePointsSub(price);


                    interactText.SetActive(false);
                }
            }
            else if (gameManager.instance.playerArmorBar.fillAmount == 1)
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
