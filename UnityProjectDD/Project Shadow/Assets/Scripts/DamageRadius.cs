using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRadius : MonoBehaviour
{
    [SerializeField] List<Collider> colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!colliders.Contains(other)) { colliders.Add(other); }
    }
    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }
    public void Damage( int damage)
    {
        Debug.Log("AOE66");
        if (colliders.Count > 0)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                IDamage dmg = colliders[i].transform.GetComponent<IDamage>();

                if (dmg != null)
                {
                    dmg.takeDamage(damage);


                }

            }
        }
        else { Debug.Log("no objects"); }
    }
    public List<Collider> GetColliders() 
    { 
        return colliders; 
    }

}
