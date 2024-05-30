using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IDamage
{
    [SerializeField] int damage;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioSource aud;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boom");
        {
            IDamage dmg = other.transform.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
                StartCoroutine(PlaySoundWithDelay());
                //Destroy(gameObject);
            }

        }
    }
    public void takeDamage(int damage)
    {
        GetComponent<Renderer>().enabled = false;
        transform.parent.gameObject.GetComponent<Renderer>().enabled = false; 
        Debug.Log("destroyed");
        StartCoroutine(PlaySoundWithDelay());
        

    }

    private IEnumerator PlaySoundWithDelay()
    {
        
        aud.PlayOneShot(explosion, 1f);
        yield return new WaitForSeconds(1);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
