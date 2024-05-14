using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIbullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.gameObject.GetComponent<EnemyAI>())
            return;
       
        IDamage dmg = other.gameObject.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
