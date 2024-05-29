using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] AudioClip explosion;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boom");
        {
            IDamage dmg = other.transform.GetComponent<IDamage>();
              
            if (dmg != null )
            dmg.takeDamage(damage);
            gameManager.instance.PlaySound(explosion, 1.0f);
            Destroy(gameObject);

        }
    }

}
