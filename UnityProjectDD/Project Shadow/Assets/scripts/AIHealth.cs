using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform position;


    public void updateHealthBar(float HP, float maxHP)
    {
        slider.value = HP / maxHP;
    }
  
    void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
}
