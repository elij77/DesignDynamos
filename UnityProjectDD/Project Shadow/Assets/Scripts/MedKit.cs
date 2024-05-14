using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{

    [SerializeField] int health;
    public GameObject player;
    public playerController playerScript;
    int hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.gameObject.GetComponent<IDamage>();
            // Increase the players health
            playerScript = other.gameObject.GetComponent<playerController>();

            //todo modify player object to not allow health to increase over orighp.
            //if (playerScript.HP + health > playerScript.HPOrig)
            //{
            //    dmg.takeDamage(playerScript.HPOrig - playerScript.HP * -1);
            //}
            //else
            //{
                dmg.takeDamage(health * -1);
            //}
            Destroy(gameObject);

        }
    }

}
