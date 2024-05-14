using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{

    [SerializeField] int damage;
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
        Debug.Log("boom");
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.gameObject.GetComponent<IDamage>();
            // decreases the players health

            dmg.takeDamage(damage);
            Destroy(gameObject);

        }
    }

}
