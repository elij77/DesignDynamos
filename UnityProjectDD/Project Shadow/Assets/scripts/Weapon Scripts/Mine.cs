using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IDamage
{
    [SerializeField] int damage;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioSource aud;
    [SerializeField] int aoeDamageAmount;
    [SerializeField] Transform damageRadius;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("boom");
        {
            IDamage dmg = other.transform.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
                aoeDamange();
                StartCoroutine(PlaySoundWithDelay());
                //Destroy(gameObject);
            }

        }
    }
    public void aoeDamange()
    {
        Debug.Log("AOE");
        DamageRadius dr;
        dr = damageRadius.GetComponent<DamageRadius>();
        if (dr != null)
        {
            Debug.Log("not found1");
            dr.Damage(aoeDamageAmount);
        }
        else { Debug.Log("not found"); }
    }
    public void takeDamage(int damage)
    {
        GetComponent<Renderer>().enabled = false;
        if (transform.parent != null)
            transform.parent.gameObject.GetComponent<Renderer>().enabled = false;
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        Debug.Log("destroyed");
        aoeDamange();
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
