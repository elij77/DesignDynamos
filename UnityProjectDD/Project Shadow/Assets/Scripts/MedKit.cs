using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{

    [SerializeField] int health;
    public GameObject player;
    public playerController playerScript;  // no need for this 
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
            IHeal heal = other.gameObject.GetComponent<IHeal>();
            // Increase the players health

            heal.Heal(health);
           
            Destroy(gameObject);

        }
    }

}
