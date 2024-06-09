using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Toggler : MonoBehaviour
{

    public GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        box.SetActive(true);
    }

    public void Toggle()
    {
        if (box.activeSelf == true)
        {
            box.SetActive(false);
        }
        else
        {
            box.SetActive(true);
        }
    }

}
