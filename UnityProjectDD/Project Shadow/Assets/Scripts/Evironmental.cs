using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evironmental : MonoBehaviour
{

    [SerializeField] int runSpeed;
    [SerializeField] int walkSpeed;
    [SerializeField] int jumpSpeed;

    private void OnTriggerEnter(Collider other)
    {
        IChangeStat ics = other.transform.GetComponent<IChangeStat>();
        if (ics != null)
        {
            
            ics.changeStat ("runSpeed", runSpeed);
            ics.changeStat("walkSpeed", walkSpeed);
            ics.changeStat("jumpSpeed", jumpSpeed);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        IChangeStat ics = other.transform.GetComponent<IChangeStat>();
        if (ics != null)
        {
            
            ics.changeStat("runSpeed", -1 * runSpeed);
            ics.changeStat("walkSpeed", -1 * walkSpeed);
            ics.changeStat("jumpSpeed", -1 * jumpSpeed);
        }
    }
}
